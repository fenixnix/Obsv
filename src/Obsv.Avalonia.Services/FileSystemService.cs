using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Obsv.Avalonia.Services;

/// <summary>
/// Implementation of file system operations
/// </summary>
public class FileSystemService : IFileSystemService
{
    /// <summary>
    /// Reads a file and returns its content
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>File information including content</returns>
    public async Task<Obsv.Avalonia.Models.FileInfo> ReadFileAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        var fileInfo = new Obsv.Avalonia.Models.FileInfo
        {
            Path = path,
            Name = Path.GetFileName(path),
            Encoding = "UTF-8"
        };

        // Check if it's an image file
        var imageMimeType = GetImageMimeType(path);
        if (imageMimeType != null)
        {
            fileInfo.IsImage = true;
            var bytes = await File.ReadAllBytesAsync(path);
            fileInfo.ImageData = Convert.ToBase64String(bytes);
            fileInfo.Size = bytes.Length;
            fileInfo.Lines = 0;
            fileInfo.Content = string.Empty;
            return fileInfo;
        }

        // Read as text file
        var content = await File.ReadAllTextAsync(path, Encoding.UTF8);
        fileInfo.Content = content;
        fileInfo.Size = new FileInfo(path).Length;
        fileInfo.Lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        fileInfo.IsImage = false;
        fileInfo.ImageData = null;

        return fileInfo;
    }

    /// <summary>
    /// Lists directory contents
    /// </summary>
    /// <param name="path">The directory path</param>
    /// <returns>List of file entries</returns>
    public async Task<IEnumerable<Obsv.Avalonia.Models.FileEntry>> ReadDirectoryAsync(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"Directory not found: {path}");

        var entries = new List<Obsv.Avalonia.Models.FileEntry>();

        // Add parent directory entry if not at root
        var directoryInfo = new DirectoryInfo(path);
        if (directoryInfo.Parent != null)
        {
            entries.Add(new Obsv.Avalonia.Models.FileEntry
            {
                Name = "..",
                Path = directoryInfo.Parent.FullName,
                IsDirectory = true
            });
        }

        // Add files and directories
        var directoryEntries = await Task.Run(() => 
            Directory.GetFileSystemEntries(path)
                .Where(e => !Path.GetFileName(e).StartsWith(".")) // Skip hidden files
                .Where(e => 
                {
                    var fileName = Path.GetFileName(e);
                    return !new[] { "node_modules", "dist", "build", "target" }.Contains(fileName);
                })
                .ToList());

        foreach (var entryPath in directoryEntries)
        {
            var isDirectory = Directory.Exists(entryPath);
            
            entries.Add(new Obsv.Avalonia.Models.FileEntry
            {
                Name = Path.GetFileName(entryPath),
                Path = entryPath,
                IsDirectory = isDirectory
            });
        }

        // Sort: directories first, then alphabetically
        entries.Sort((a, b) =>
        {
            if (a.IsDirectory && !b.IsDirectory) return -1;
            if (!a.IsDirectory && b.IsDirectory) return 1;
            return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
        });

        return entries;
    }

    /// <summary>
    /// Checks if a file exists
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>True if file exists</returns>
    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// Gets the MIME type for image files
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>MIME type or null if not an image</returns>
    private string? GetImageMimeType(string path)
    {
        var extension = Path.GetExtension(path)?.ToLowerInvariant().TrimStart('.');
        return extension switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "bmp" => "image/bmp",
            "webp" => "image/webp",
            "avif" => "image/avif",
            "ico" => "image/x-icon",
            "svg" => "image/svg+xml",
            _ => null
        };
    }
}
