using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Application.Services;

internal class FolderService : IFolderService
{
    private readonly AppDbContext _dbContext;
    private readonly IEventBus _eventBus;
    private readonly IFileService _fileService;
    private readonly IUserStorageService _userStorageService;

    public FolderService(AppDbContext dbContext, IEventBus eventBus, IFileService fileService, IUserStorageService userStorageService)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
        _fileService = fileService;
        _userStorageService = userStorageService;
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
        await _eventBus.PublishAsync(new FileFolderAccessed(folder), cancellationToken);
        return folder;
    }

    public async Task<FileFolder> GetMetadataAsync(string userId, string idOrPath,
        CancellationToken cancellationToken = default)
    {
        var folder = Guid.TryParse(idOrPath, out var folderId)
            ? _dbContext.FileFolders.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Id == folderId &&
                x.Type == FileFolderType.Folder
            )
            : _dbContext.FileFolders.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Path == idOrPath &&
                x.Type == FileFolderType.Folder
            );

        if (folder is null) throw new FolderNotFoundException(userId, folderId);
        await _eventBus.PublishAsync(new FileFolderAccessed(folder), cancellationToken);
        return folder;
    }

    public async Task<IEnumerable<FileFolder>> GetChildrenAsync(string userId, Guid? folderId,
        CancellationToken cancellationToken = default)
    {
        var folder = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId &&
                                                                x.Id == folderId &&
                                                                x.Type == FileFolderType.Folder &&
                                                                !x.IsInTrash);
        if (folderId is not null && folder is null)
        {
            throw new FolderNotFoundException(userId, folderId.Value);
        }

        var children = _dbContext.FileFolders.Where(x => x.UserId == userId && x.ParentId == folderId && !x.IsInTrash)
            .ToList();

        if (folderId is not null)
        {
            await _eventBus.PublishAsync(new FileFolderAccessed(folder!), cancellationToken);
        }

        return children;
    }

    public async Task<FileFolder> RenameAsync(string userId, Guid folderId, string newFolderName,
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
        await _eventBus.PublishAsync(new FileFolderAccessed(folder), cancellationToken);
        await _eventBus.PublishAsync(new FileFolderAccessed(folder), cancellationToken);
        return folder;
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
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var folder = _dbContext.FileFolders.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Id == folderId &&
                x.Type == FileFolderType.Folder
            );
            if (folder is null) throw new FolderNotFoundException(userId, folderId);

            var userStorage = await _dbContext.UserStorages
                .FromSqlRaw("SELECT * FROM UserStorages WITH (UPDLOCK, ROWLOCK) WHERE UserId = {0}", userId)
                .FirstAsync(cancellationToken: cancellationToken);

            
            await DeleteFolderAndDescendantsAsync(userId, folder, userStorage, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _userStorageService.UpdateAsync(userStorage, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteFolderAndDescendantsAsync(string userId,
        FileFolder folder, UserStorage userStorage, CancellationToken cancellationToken = default)
    {
        // Find all descendants of this folder
        var descendants = _dbContext.FileFolders.Where(x =>
            x.UserId == userId && x.Path.StartsWith(folder.Path + "/")).ToList();

        // Remove all descendants first
        foreach (var file in descendants.Where(x => x.Type == FileFolderType.File))
        {
            await _fileService.DeleteFileAsync(file, userStorage, cancellationToken);
        }

        foreach (var descendantFolder in descendants.Where(x => x.Type == FileFolderType.Folder))
        {
            await DeleteFolderAndDescendantsAsync(userId, descendantFolder, userStorage, cancellationToken);
        }

        // Then remove the folder itself
        _dbContext.FileFolders.Remove(folder);
    }

    public async Task<FileFolder> RestoreFromTrashAsync(string userId, Guid folderId,
        CancellationToken cancellationToken = default)
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
        return folder;
    }
}