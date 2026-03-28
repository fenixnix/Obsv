using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Obsv.Avalonia.Models;

namespace Obsv.Avalonia.ViewModels;

/// <summary>
/// Editor view model for file content display
/// </summary>
public partial class EditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _fileContent;

    [ObservableProperty]
    private bool _isImage;

    [ObservableProperty]
    private string? _imageData;

    [ObservableProperty]
    private bool _wordWrap = true;

    [ObservableProperty]
    private string? _fileName;

    /// <summary>
    /// Loads file content into the editor
    /// </summary>
    /// <param name="fileInfo">File information to display</param>
    public void LoadFile(Obsv.Avalonia.Models.FileInfo fileInfo)
    {
        if (fileInfo == null)
        {
            FileContent = null;
            IsImage = false;
            ImageData = null;
            FileName = null;
            return;
        }

        FileName = fileInfo.Name;
        IsImage = fileInfo.IsImage;

        if (fileInfo.IsImage)
        {
            ImageData = fileInfo.ImageData;
            FileContent = string.Empty;
        }
        else
        {
            FileContent = fileInfo.Content;
            ImageData = null;
        }
    }

    /// <summary>
    /// Toggles word wrap setting
    /// </summary>
    [RelayCommand]
    private void ToggleWordWrap()
    {
        WordWrap = !WordWrap;
    }
}
