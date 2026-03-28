using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Obsv.Avalonia.Models;
using Obsv.Avalonia.Services;

namespace Obsv.Avalonia.ViewModels;

/// <summary>
/// Main window view model
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IThemeService _themeService;

    [ObservableProperty]
    private string? _rootPath;

    [ObservableProperty]
    private FileEntry? _selectedFile;

    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private EditorViewModel? _editorViewModel;

    [ObservableProperty]
    private string? _currentThemeName;

    public MainWindowViewModel(IFileSystemService fileSystemService, IThemeService themeService)
    {
        _fileSystemService = fileSystemService;
        _themeService = themeService;
        EditorViewModel = new EditorViewModel();
        _currentThemeName = _themeService.CurrentTheme.Name;
        StatusMessage = "Ready";
    }

    [RelayCommand]
    private async Task OpenFolderAsync()
    {
        // TODO: Implement folder selection dialog
        StatusMessage = "Open folder command executed";
    }

    public async Task LoadFileAsync(FileEntry fileEntry)
    {
        if (fileEntry == null || fileEntry.IsDirectory)
            return;

        SelectedFile = fileEntry;
        StatusMessage = $"Loading {fileEntry.Name}...";

        try
        {
            var fileInfo = await _fileSystemService.ReadFileAsync(fileEntry.Path);
            EditorViewModel?.LoadFile(fileInfo);
            StatusMessage = $"Loaded {fileEntry.Name}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading file: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ChangeTheme(string themeName)
    {
        _themeService.SetTheme(themeName);
        CurrentThemeName = _themeService.CurrentTheme.Name;
        StatusMessage = $"Theme changed to {themeName}";
    }
}
