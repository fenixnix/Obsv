using Obsv.Avalonia.Models;

namespace Obsv.Avalonia.Services;

/// <summary>
/// Interface for theme management
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets the current theme
    /// </summary>
    AppTheme CurrentTheme { get; }

    /// <summary>
    /// Gets the list of built-in themes
    /// </summary>
    IReadOnlyList<AppTheme> BuiltInThemes { get; }

    /// <summary>
    /// Sets the current theme by name
    /// </summary>
    /// <param name="themeName">The name of the theme to set</param>
    void SetTheme(string themeName);

    /// <summary>
    /// Sets a custom theme
    /// </summary>
    /// <param name="customTheme">The custom theme to set</param>
    void SetCustomTheme(AppTheme customTheme);

    /// <summary>
    /// Loads a theme from a configuration file
    /// </summary>
    /// <param name="filePath">Path to the theme configuration file</param>
    /// <returns>The loaded theme</returns>
    Task<AppTheme> LoadThemeFromFileAsync(string filePath);

    /// <summary>
    /// Saves the current theme to a configuration file
    /// </summary>
    /// <param name="filePath">Path to save the theme configuration</param>
    Task SaveThemeToFileAsync(string filePath);
}
