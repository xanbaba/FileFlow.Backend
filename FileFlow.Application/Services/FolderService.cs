using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;

namespace FileFlow.Application.Services;

internal class FolderService : IFolderService
{
    private readonly AppDbContext _dbContext;

    public FolderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FileFolder> CreateAsync(string userId, string folderName, Guid? targetFolderId,
        CancellationToken cancellationToken = default)
    {
        Guid? parentId = null;
        FileFolder? parent = null;
        if (targetFolderId is not null)
        {
            parent = _dbContext.FileFolders.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Id == targetFolderId &&
                x.Type == FileFolderType.Folder &&
                !x.IsInTrash
            );
            if (parent is null) throw new FolderNotFoundException(userId, targetFolderId.Value);
            parentId = parent.Id;
        }

        var path = Path.Join(parent?.Path ?? string.Empty, folderName);
        if (_dbContext.FileFolders.Any(x => x.UserId == userId && x.Path == path))
        {
            throw new FolderAlreadyExistsException(userId, path);
        }
        var folder = new FileFolder
        {
            Id = Guid.NewGuid(),
            IsStarred = false,
            IsInTrash = false,
            UserId = userId,
            Name = folderName,
            Path = path,
            Size = 0,
            Type = FileFolderType.Folder,
            ParentId = parentId,
            FileCategory = FileCategory.Other
        };

        _dbContext.FileFolders.Add(folder);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return folder;
    }

    public Task<FileFolder> GetMetadataAsync(string userId, Guid folderId,
        CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x =>
            x.UserId == userId &&
            x.Id == folderId &&
            x.Type == FileFolderType.Folder
        );
        if (folder is null) throw new FolderNotFoundException(userId, folderId);
        return Task.FromResult(folder);
    }

    public Task<IEnumerable<FileFolder>> GetChildrenAsync(string userId, Guid? folderId,
        CancellationToken cancellationToken = default)
    {
        if (folderId is not null && !_dbContext.FileFolders.Any(x => x.UserId == userId &&
                                                                     x.Id == folderId &&
                                                                     x.Type == FileFolderType.Folder &&
                                                                     !x.IsInTrash))
        {
            throw new FolderNotFoundException(userId, folderId.Value);
        }

        var children = _dbContext.FileFolders.Where(x => x.UserId == userId && x.ParentId == folderId).ToList();
        return Task.FromResult<IEnumerable<FileFolder>>(children);
    }

    public async Task RenameAsync(string userId, Guid folderId, string newFolderName,
        CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x =>
            x.UserId == userId &&
            x.Id == folderId &&
            !x.IsInTrash &&
            x.Type == FileFolderType.Folder
        );
        if (folder is null) throw new FolderNotFoundException(userId, folderId);

        string oldPath = folder.Path;

        // Update folder's name
        folder.Name = newFolderName;

        // Update folder's path
        folder.Path = string.Join('/', folder.Path.Split('/').SkipLast(1).Append(newFolderName));

        // Update paths of all descendant files and folders
        var descendants = _dbContext.FileFolders.Where(x =>
            x.UserId == userId && x.Path.StartsWith(oldPath + "/")).ToList();

        foreach (var descendant in descendants)
        {
            descendant.Path = descendant.Path.Replace(oldPath, folder.Path);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MoveToTrashAsync(string userId, Guid folderId, CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x =>
            x.UserId == userId &&
            x.Id == folderId &&
            x.Type == FileFolderType.Folder
        );
        if (folder is null) throw new FolderNotFoundException(userId, folderId);

        folder.IsInTrash = true;

        // Also move all descendants to trash
        var descendants = _dbContext.FileFolders.Where(x =>
            x.UserId == userId && x.Path.StartsWith(folder.Path + "/")).ToList();

        foreach (var descendant in descendants)
        {
            descendant.IsInTrash = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePermanentlyAsync(string userId, Guid folderId,
        CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x =>
            x.UserId == userId &&
            x.Id == folderId &&
            x.Type == FileFolderType.Folder
        );
        if (folder is null) throw new FolderNotFoundException(userId, folderId);

        // Find all descendants of this folder
        var descendants = _dbContext.FileFolders.Where(x =>
            x.UserId == userId && x.Path.StartsWith(folder.Path + "/")).ToList();

        // Remove all descendants first
        _dbContext.FileFolders.RemoveRange(descendants);

        // Then remove the folder itself
        _dbContext.FileFolders.Remove(folder);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreFromTrashAsync(string userId, Guid folderId, CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x =>
            x.UserId == userId &&
            x.Id == folderId &&
            x.Type == FileFolderType.Folder
        );
        if (folder is null) throw new FolderNotFoundException(userId, folderId);

        folder.IsInTrash = false;

        // Also restore all descendants from trash
        var descendants = _dbContext.FileFolders.Where(x =>
            x.UserId == userId && x.Path.StartsWith(folder.Path + "/")).ToList();

        foreach (var descendant in descendants)
        {
            descendant.IsInTrash = false;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}