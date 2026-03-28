using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Obsv.Avalonia.Models;
using Obsv.Avalonia.Services;

namespace Obsv.Avalonia.ViewModels;

/// <summary>
/// File tree view model for sidebar
/// </summary>
public partial class FileTreeViewModel : ObservableObject
{
    private readonly IFileSystemService _fileSystemService;
    private readonly MainWindowViewModel _mainViewModel;

    [ObservableProperty]
    private IEnumerable<FileEntry> _fileEntries = Enumerable.Empty<FileEntry>();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _currentPath;

    public FileTreeViewModel(IFileSystemService fileSystemService, MainWindowViewModel mainViewModel)
    {
        _fileSystemService = fileSystemService;
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private async Task LoadDirectoryAsync(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        IsLoading = true;
        CurrentPath = path;

        try
        {
            FileEntries = await _fileSystemService.ReadDirectoryAsync(path);
            // Update main view model's root path
            _mainViewModel.RootPath = path;
        }
        catch (Exception ex)
        {
            // TODO: Handle error appropriately
            FileEntries = Enumerable.Empty<FileEntry>();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateUpAsync()
    {
        if (string.IsNullOrEmpty(CurrentPath))
            return;

        var parent = Directory.GetParent(CurrentPath);
        if (parent != null)
        {
            await LoadDirectoryAsync(parent.FullName);
        }
    }

    [RelayCommand]
    private async Task SelectFileAsync(FileEntry fileEntry)
    {
        if (fileEntry == null)
            return;

        if (fileEntry.IsDirectory)
        {
            await LoadDirectoryAsync(fileEntry.Path);
        }
        else
        {
            await _mainViewModel.LoadFileAsync(fileEntry);
        }
    }
}
