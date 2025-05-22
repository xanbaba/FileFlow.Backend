namespace FileFlow.Application.Database.Entities;

public enum FileFolderType
{
    File,
    Folder
}

public enum FileType
{
    Document,
    Image,
    Video,
    Other
}

/// <summary>
/// Represents a file or folder entity in the application.
/// </summary>
public class FileFolder
{
    /// <summary>
    /// Gets or sets the unique identifier for the file or folder entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the user who owns or is linked to the file or folder entity.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the file or folder, including the extension for files.
    /// </summary>
    public string Name { get; set; } = null!; // Includes extension (for files)

    /// <summary>
    /// Gets or sets the type of the entity, indicating whether it is a file or folder.
    /// </summary>
    public FileFolderType Type { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the file or folder is marked as starred by the user.
    /// </summary>
    public bool IsStarred { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent folder. Null if the item is in the root directory.
    /// </summary>
    public Guid? ParentId { get; set; } // ID of parent folder, null if in root

    /// <summary>
    /// Gets or sets the full path to the file or folder within the user's storage.
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// Gets or sets the size of the file in megabytes (MB). Null for folders.
    /// </summary>
    public int? Size { get; set; } // MB

    /// <summary>
    /// Gets or sets the type of the file (document, image, video, or other). Null for folders.
    /// </summary>
    public FileType? FileType { get; set; } // For files

    /// <summary>
    /// Gets or sets a value indicating whether the file or folder is currently in the trash.
    /// </summary>
    public bool IsInTrash { get; set; }
}