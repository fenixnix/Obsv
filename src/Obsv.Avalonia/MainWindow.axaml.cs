using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using Obsv.Avalonia.Services;
using Obsv.Avalonia.ViewModels;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace Obsv.Avalonia;

public partial class MainWindow : Window
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IThemeService _themeService;
    private string? _currentPath;
    private string? _currentExtension;
    private TextEditor? _editor;
    private bool _isDarkTheme = true;
    
    // Custom highlighting definitions
    private IHighlightingDefinition? _darkHighlighting;
    private IHighlightingDefinition? _lightHighlighting;

    public MainWindow()
    {
        InitializeComponent();
        
        // Initialize services
        _fileSystemService = new FileSystemService();
        _themeService = new ThemeService();
        
        // Get editor reference after InitializeComponent
        _editor = this.FindControl<TextEditor>("EditorContent");
        
        // Load custom XSHD highlighting definitions
        LoadCustomHighlighting();
        
        // Set initial text - use Dark theme
        ApplyTheme(isDark: true);
        
        if (_editor != null)
        {
            _editor.Text = "Welcome to Obsv Reader!\n\nUse File > Open Folder to browse files";
        }
        
        // Set DataContext
        DataContext = new MainWindowViewModel(_fileSystemService, _themeService);
    }

    private void LoadCustomHighlighting()
    {
        try
        {
            // Get the bin folder path directly
            var binPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            var darkPath = System.IO.Path.Combine(binPath, "Highlighting", "Dark.xshd");
            var lightPath = System.IO.Path.Combine(binPath, "Highlighting", "Light.xshd");
            
            // Load Dark theme XSHD
            if (System.IO.File.Exists(darkPath))
            {
                using var reader = new XmlTextReader(darkPath);
                _darkHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            
            // Load Light theme XSHD
            if (System.IO.File.Exists(lightPath))
            {
                using var reader = new XmlTextReader(lightPath);
                _lightHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading custom highlighting: {ex.Message}");
        }
    }

    private async void OpenFolder_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Folder",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            _currentPath = folders[0].Path.LocalPath;
            await LoadDirectoryAsync(_currentPath);
        }
    }

    private async Task LoadDirectoryAsync(string path)
    {
        try
        {
            var entries = await _fileSystemService.ReadDirectoryAsync(path);
            
            var content = this.FindControl<StackPanel>("SidebarContent");
            if (content != null)
            {
                content.Children.Clear();
                
                // Note: ".." parent directory is already included in entries from FileSystemService
                
                foreach (var entry in entries)
                {
                    var displayName = entry.Name == ".." ? "📁 .. (Parent)" : 
                                      (entry.IsDirectory ? $"📁 {entry.Name}" : $"📄 {entry.Name}");
                    var item = new Button
                    {
                        Content = displayName,
                        Tag = entry,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };
                    
                    if (entry.IsDirectory)
                    {
                        item.Click += async (s, ev) => {
                            if (s is Button btn && btn.Tag is Obsv.Avalonia.Models.FileEntry fe)
                                await LoadDirectoryAsync(fe.Path);
                        };
                    }
                    else
                    {
                        item.Click += async (s, ev) => {
                            if (s is Button btn && btn.Tag is Obsv.Avalonia.Models.FileEntry fe)
                                await OpenFileAsync(fe.Path);
                        };
                    }
                    
                    content.Children.Add(item);
                }
            }
            
            var pathText = this.FindControl<TextBlock>("CurrentPathText");
            if (pathText != null)
                pathText.Text = path;
            
            UpdateEditorArea("Folder loaded: " + path, null);
        }
        catch (System.Exception ex)
        {
            UpdateEditorArea("Error: " + ex.Message, null);
        }
    }

    private async Task OpenFileAsync(string path)
    {
        try
        {
            var fileInfo = await _fileSystemService.ReadFileAsync(path);
            
            if (fileInfo.IsImage)
            {
                UpdateEditorArea($"[Image: {fileInfo.Name}, Size: {fileInfo.Size} bytes]", null);
            }
            else
            {
                // Get file extension for syntax highlighting
                _currentExtension = System.IO.Path.GetExtension(path).ToLowerInvariant();
                UpdateEditorArea(fileInfo.Content, _currentExtension);
                
                var fileNameText = this.FindControl<TextBlock>("FileNameText");
                if (fileNameText != null)
                    fileNameText.Text = fileInfo.Name;
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OpenFileAsync error: {ex.Message}");
            UpdateEditorArea("Error loading file: " + ex.Message, null);
        }
    }

    private void UpdateEditorArea(string text, string? extension)
    {
        try
        {
            if (_editor != null && _editor.Document != null)
            {
                _editor.Document.Text = text ?? string.Empty;
                
                // Set syntax highlighting based on extension and current theme
                if (!string.IsNullOrEmpty(extension))
                {
                    _editor.SyntaxHighlighting = GetHighlighting(extension);
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateEditorArea error: {ex.Message}");
        }
    }

    private IHighlightingDefinition? GetHighlighting(string extension)
    {
        try
        {
            // Use custom XSHD highlighting based on current theme
            var customHighlighting = _isDarkTheme ? _darkHighlighting : _lightHighlighting;
            
            if (customHighlighting != null)
            {
                return customHighlighting;
            }
            
            // Fallback to built-in highlighting
            return GetBuiltInHighlighting(extension);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetHighlighting error: {ex.Message}");
            return null;
        }
    }

    private string GetLanguageName(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".js" or ".jsx" or ".mjs" => "JavaScript",
            ".ts" or ".tsx" or ".mts" => "TypeScript",
            ".py" or ".pyw" => "Python",
            ".rs" => "Rust",
            ".java" => "Java",
            ".c" or ".h" => "C++",
            ".cpp" or ".cc" or ".cxx" or ".hpp" => "C++",
            ".cs" => "C#",
            ".go" => "Go",
            ".rb" => "Ruby",
            ".php" => "PHP",
            ".swift" => "Swift",
            ".html" or ".htm" => "HTML",
            ".css" or ".scss" or ".sass" => "CSS",
            ".json" => "Json",
            ".xml" => "XML",
            ".yaml" or ".yml" => "YAML",
            ".md" or ".markdown" => "MarkDown",
            ".sql" => "TSQL",
            ".sh" or ".bash" or ".zsh" => "Boo",
            ".lua" => "Lua",
            ".ps1" => "PowerShell",
            _ => "JavaScript"
        };
    }

    private IHighlightingDefinition? GetBuiltInHighlighting(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".js" or ".jsx" or ".mjs" => HighlightingManager.Instance.GetDefinition("JavaScript"),
            ".ts" or ".tsx" or ".mts" => HighlightingManager.Instance.GetDefinition("TypeScript"),
            ".py" or ".pyw" => HighlightingManager.Instance.GetDefinition("Python"),
            ".rs" => HighlightingManager.Instance.GetDefinition("Rust"),
            ".java" => HighlightingManager.Instance.GetDefinition("Java"),
            ".c" or ".h" => HighlightingManager.Instance.GetDefinition("C++"),
            ".cpp" or ".cc" or ".cxx" or ".hpp" => HighlightingManager.Instance.GetDefinition("C++"),
            ".cs" => HighlightingManager.Instance.GetDefinition("C#"),
            ".go" => HighlightingManager.Instance.GetDefinition("Go"),
            ".rb" => HighlightingManager.Instance.GetDefinition("Ruby"),
            ".php" => HighlightingManager.Instance.GetDefinition("PHP"),
            ".swift" => HighlightingManager.Instance.GetDefinition("Swift"),
            ".html" or ".htm" => HighlightingManager.Instance.GetDefinition("HTML"),
            ".css" or ".scss" or ".sass" => HighlightingManager.Instance.GetDefinition("CSS"),
            ".json" => HighlightingManager.Instance.GetDefinition("Json"),
            ".xml" => HighlightingManager.Instance.GetDefinition("XML"),
            ".yaml" or ".yml" => HighlightingManager.Instance.GetDefinition("YAML"),
            ".md" or ".markdown" => HighlightingManager.Instance.GetDefinition("MarkDown"),
            ".sql" => HighlightingManager.Instance.GetDefinition("TSQL"),
            ".sh" or ".bash" or ".zsh" => HighlightingManager.Instance.GetDefinition("Boo"),
            ".lua" => HighlightingManager.Instance.GetDefinition("Lua"),
            ".ps1" => HighlightingManager.Instance.GetDefinition("PowerShell"),
            _ => HighlightingManager.Instance.GetDefinition("JavaScript")
        };
    }

    private void WrapToggle_Click(object? sender, RoutedEventArgs e)
    {
        if (_editor != null && sender is ToggleButton btn)
        {
            _editor.WordWrap = btn.IsChecked == true;
        }
    }

    private void ThemeDark_Click(object? sender, RoutedEventArgs e)
    {
        ApplyTheme(isDark: true);
    }

    private void ThemeLight_Click(object? sender, RoutedEventArgs e)
    {
        ApplyTheme(isDark: false);
    }

    private void ApplyTheme(bool isDark)
    {
        // Track current theme
        _isDarkTheme = isDark;
        
        // Apply UI colors
        if (isDark)
        {
            this.Background = new SolidColorBrush(Color.Parse("#FF1E1E1E"));
            
            if (_editor != null)
            {
                _editor.Background = new SolidColorBrush(Color.Parse("#FF1E1E1E"));
                _editor.Foreground = new SolidColorBrush(Color.Parse("#FFD4D4D4"));
            }
        }
        else
        {
            this.Background = new SolidColorBrush(Color.Parse("#FFFFFFFF"));
            
            if (_editor != null)
            {
                _editor.Background = new SolidColorBrush(Color.Parse("#FFFFFFFF"));
                _editor.Foreground = new SolidColorBrush(Color.Parse("#FF000000"));
            }
        }
        
        // Reapply syntax highlighting if a file is loaded
        if (_editor != null && !string.IsNullOrEmpty(_currentExtension))
        {
            _editor.SyntaxHighlighting = GetHighlighting(_currentExtension);
        }
    }

    private void Resize_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Placeholder for sidebar resize functionality
    }

    private void About_Click(object? sender, RoutedEventArgs e)
    {
        // Show about dialog
    }
}
