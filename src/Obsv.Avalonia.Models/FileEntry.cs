namespace Obsv.Avalonia.Models;

/// <summary>
/// Represents a file or directory entry
/// </summary>
public class FileEntry
{
    /// <summary>
    /// Name of the file or directory
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Full path to the file or directory
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Whether this entry is a directory
    /// </summary>
    public bool IsDirectory { get; set; }

    /// <summary>
    /// Whether this entry is expanded in UI (for tree view)
    /// </summary>
    public bool Expanded { get; set; }

    /// <summary>
    /// Child entries (if this is a directory)
    /// </summary>
    public List<FileEntry> Children { get; set; } = new();
}
