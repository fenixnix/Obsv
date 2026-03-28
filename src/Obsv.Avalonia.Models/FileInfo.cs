using System.Text;

namespace Obsv.Avalonia.Models;

/// <summary>
/// Represents detailed file information
/// </summary>
public class FileInfo
{
    /// <summary>
    /// Full path to the file
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// File name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// File content as string
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// File encoding
    /// </summary>
    public string Encoding { get; set; } = "UTF-8";

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Number of lines in the file
    /// </summary>
    public int Lines { get; set; }

    /// <summary>
    /// Whether this file is an image
    /// </summary>
    public bool IsImage { get; set; }

    /// <summary>
    /// Base64 encoded image data (if IsImage is true)
    /// </summary>
    public string? ImageData { get; set; }

    /// <summary>
    /// Gets the file extension
    /// </summary>
    public string Extension => Path.Split('.').LastOrDefault()?.ToLowerInvariant() ?? string.Empty;

    /// <summary>
    /// Gets the MIME type for image files
    /// </summary>
    public string? GetImageMimeType()
    {
        if (!IsImage) return null;

        return Extension.ToLowerInvariant() switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "bmp" => "image/bmp",
            "webp" => "image/webp",
            "avif" => "image/avif",
            "ico" => "image/x-icon",
            "svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}
