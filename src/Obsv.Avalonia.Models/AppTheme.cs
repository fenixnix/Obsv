using System.ComponentModel;

namespace Obsv.Avalonia.Models;

/// <summary>
/// Represents a theme configuration
/// </summary>
public class AppTheme : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _backgroundPrimary = "#FF1E1E1E";
    private string _backgroundSecondary = "#FF2D2D2D";
    private string _textPrimary = "#FFFFFFFF";
    private string _accent = "#FF00FF00";
    private string _border = "#FF3F3F3F";
    private string _editorBackground = "#FF1E1E1E";
    private string _editorForeground = "#FFFFFFFF";

    /// <summary>
    /// Theme name
    /// </summary>
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(nameof(Name)); }
    }

    /// <summary>
    /// Primary background color
    /// </summary>
    public string BackgroundPrimary
    {
        get => _backgroundPrimary;
        set { _backgroundPrimary = value; OnPropertyChanged(nameof(BackgroundPrimary)); }
    }

    /// <summary>
    /// Secondary background color
    /// </summary>
    public string BackgroundSecondary
    {
        get => _backgroundSecondary;
        set { _backgroundSecondary = value; OnPropertyChanged(nameof(BackgroundSecondary)); }
    }

    /// <summary>
    /// Primary text color
    /// </summary>
    public string TextPrimary
    {
        get => _textPrimary;
        set { _textPrimary = value; OnPropertyChanged(nameof(TextPrimary)); }
    }

    /// <summary>
    /// Accent color
    /// </summary>
    public string Accent
    {
        get => _accent;
        set { _accent = value; OnPropertyChanged(nameof(Accent)); }
    }

    /// <summary>
    /// Border color
    /// </summary>
    public string Border
    {
        get => _border;
        set { _border = value; OnPropertyChanged(nameof(Border)); }
    }

    /// <summary>
    /// Editor background color
    /// </summary>
    public string EditorBackground
    {
        get => _editorBackground;
        set { _editorBackground = value; OnPropertyChanged(nameof(EditorBackground)); }
    }

    /// <summary>
    /// Editor foreground color
    /// </summary>
    public string EditorForeground
    {
        get => _editorForeground;
        set { _editorForeground = value; OnPropertyChanged(nameof(EditorForeground)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Creates a copy of this theme
    /// </summary>
    public AppTheme Clone()
    {
        return (AppTheme)MemberwiseClone();
    }
}
