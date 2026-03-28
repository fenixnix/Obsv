using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Obsv.Avalonia.Models;

namespace Obsv.Avalonia.Services;

/// <summary>
/// Implementation of theme management
/// </summary>
public class ThemeService : IThemeService
{
    private AppTheme _currentTheme = new();
    private readonly ObservableCollection<AppTheme> _builtInThemes = new();

    public ThemeService()
    {
        InitializeBuiltInThemes();
        // Set default theme
        _currentTheme = _builtInThemes.FirstOrDefault(t => t.Name == "Green Essence") 
                       ?? _builtInThemes.FirstOrDefault() 
                       ?? new AppTheme { Name = "Default" };
    }

    /// <summary>
    /// Gets the current theme
    /// </summary>
    public AppTheme CurrentTheme => _currentTheme;

    /// <summary>
    /// Gets the list of built-in themes
    /// </summary>
    public IReadOnlyList<AppTheme> BuiltInThemes => new ReadOnlyCollection<AppTheme>(_builtInThemes);

    /// <summary>
    /// Sets the current theme by name
    /// </summary>
    /// <param name="themeName">The name of the theme to set</param>
    public void SetTheme(string themeName)
    {
        var theme = _builtInThemes.FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
        if (theme != null)
        {
            _currentTheme = theme.Clone();
        }
        else
        {
            // If theme not found, keep current but could log warning
        }
    }

    /// <summary>
    /// Sets a custom theme
    /// </summary>
    /// <param name="customTheme">The custom theme to set</param>
    public void SetCustomTheme(AppTheme customTheme)
    {
        if (customTheme != null)
        {
            _currentTheme = customTheme.Clone();
            _currentTheme.Name = "Custom";
        }
    }

    /// <summary>
    /// Loads a theme from a configuration file
    /// </summary>
    /// <param name="filePath">Path to the theme configuration file</param>
    /// <returns>The loaded theme</returns>
    public async Task<AppTheme> LoadThemeFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Theme file not found: {filePath}");

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var theme = JsonSerializer.Deserialize<AppTheme>(json);
            if (theme != null)
            {
                _currentTheme = theme;
                return theme;
            }
            throw new InvalidOperationException("Failed to deserialize theme");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Invalid theme file format: {ex.Message}");
        }
    }

    /// <summary>
    /// Saves the current theme to a configuration file
    /// </summary>
    /// <param name="filePath">Path to save the theme configuration</param>
    public async Task SaveThemeToFileAsync(string filePath)
    {
        var json = JsonSerializer.Serialize(_currentTheme, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }

    /// <summary>
    /// Initializes the built-in themes
    /// </summary>
    private void InitializeBuiltInThemes()
    {
        // Green Essence (Default)
        _builtInThemes.Add(new AppTheme
        {
            Name = "Green Essence",
            BackgroundPrimary = "#FF1E1E1E",
            BackgroundSecondary = "#FF2D2D2D",
            TextPrimary = "#FFFFFFFF",
            Accent = "#FF00FF00",
            Border = "#FF3F3F3F",
            EditorBackground = "#FF1E1E1E",
            EditorForeground = "#FFFFFFFF"
        });

        // Retro Console
        _builtInThemes.Add(new AppTheme
        {
            Name = "Retro Console",
            BackgroundPrimary = "#FF000000",
            BackgroundSecondary = "#FF000000",
            TextPrimary = "#FF00FF00",
            Accent = "#FF00FF00",
            Border = "#FF003300",
            EditorBackground = "#FF000000",
            EditorForeground = "#FF00FF00"
        });

        // Cyberpunk
        _builtInThemes.Add(new AppTheme
        {
            Name = "Cyberpunk",
            BackgroundPrimary = "#FF0D0D0D",
            BackgroundSecondary = "#FF1A1A1A",
            TextPrimary = "#FF00FFAA",
            Accent = "#FF00FFAA",
            Border = "#FF003322",
            EditorBackground = "#FF0D0D0D",
            EditorForeground = "#FF00FFAA"
        });

        // Twilight
        _builtInThemes.Add(new AppTheme
        {
            Name = "Twilight",
            BackgroundPrimary = "#FF1A1A2E",
            BackgroundSecondary = "#FF16213E",
            TextPrimary = "#FFE94560",
            Accent = "#FFE94560",
            Border = "#FF0F3460",
            EditorBackground = "#FF0F3460",
            EditorForeground = "#FFE94560"
        });

        // Paper Ink
        _builtInThemes.Add(new AppTheme
        {
            Name = "Paper Ink",
            BackgroundPrimary = "#FFFAF9F6",
            BackgroundSecondary = "#FFF0EDE5",
            TextPrimary = "#FF4A4A4A",
            Accent = "#FF8B4513",
            Border = "#FFD4C5A8",
            EditorBackground = "#FFFFFAF0",
            EditorForeground = "#FF4A4A4A"
        });
    }
}
