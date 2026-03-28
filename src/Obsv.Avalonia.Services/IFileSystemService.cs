namespace Obsv.Avalonia.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for file system operations
/// </summary>
public interface IFileSystemService
{
    /// <summary>
    /// Reads a file and returns its content
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>File information including content</returns>
    Task<Obsv.Avalonia.Models.FileInfo> ReadFileAsync(string path);

    /// <summary>
    /// Lists directory contents
    /// </summary>
    /// <param name="path">The directory path</param>
    /// <returns>List of file entries</returns>
    Task<IEnumerable<Obsv.Avalonia.Models.FileEntry>> ReadDirectoryAsync(string path);

    /// <summary>
    /// Checks if a file exists
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>True if file exists</returns>
    bool FileExists(string path);
}
