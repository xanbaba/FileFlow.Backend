using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Application.Services;

internal class ItemService : IItemService, IEventHandler<FileFolderAccessed>
{
    private readonly AppDbContext _dbContext;
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;
    private readonly IUserStorageService _userStorageService;

    public ItemService(AppDbContext dbContext, IFileService fileService, IFolderService folderService, IUserStorageService userStorageService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _folderService = folderService;
        _userStorageService = userStorageService;
    }

    public Task<IEnumerable<FileFolder>> GetStarredAsync(string userId, CancellationToken cancellationToken = default)
    {
        var items = _dbContext.FileFolders
            .Where(x => x.UserId == userId && x.IsStarred)
            .ToList();
        
        return Task.FromResult<IEnumerable<FileFolder>>(items);
    }

    public Task<IEnumerable<FileFolder>> GetRecentAsync(string userId, CancellationToken cancellationToken = default)
    {
        var items = _dbContext.FileFolders
            .Where(x => x.LastAccessed.HasValue && !x.IsInTrash && x.LastAccessed.Value >= DateTime.UtcNow.AddDays(-1));
        
        return Task.FromResult<IEnumerable<FileFolder>>(items);
    }

    public Task<IEnumerable<FileFolder>> GetInTrashAsync(string userId, CancellationToken cancellationToken = default)
    {
        var items = _dbContext.FileFolders
            .Where(x => x.UserId == userId && x.IsInTrash && (x.Parent == null || !x.Parent.IsInTrash))
            .ToList();
        
        return Task.FromResult<IEnumerable<FileFolder>>(items);
    }

    public async Task StarAsync(string userId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == itemId);
        if (item is null)
        {
            throw new ItemNotFoundException(userId, itemId);
        }

        item.IsStarred = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UnstarAsync(string userId, Guid itemId, CancellationToken cancellationToken = default)
    {
        var item = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == itemId);
        if (item is null)
        {
            throw new ItemNotFoundException(userId, itemId);
        }

        item.IsStarred = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task EmptyTrashAsync(string userId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var trashItems = await _dbContext.FileFolders
                .Where(x => x.UserId == userId && x.IsInTrash)
                .ToListAsync(cancellationToken: cancellationToken);
            
            var userStorage = await _dbContext.UserStorages
                .FromSqlRaw("SELECT * FROM UserStorages WITH (UPDLOCK, ROWLOCK) WHERE UserId = {0}", userId)
                .FirstAsync(cancellationToken: cancellationToken);

            foreach (var fileFolder in trashItems)
            {
                switch (fileFolder.Type)
                {
                    case FileFolderType.File:
                        await _fileService.DeleteFileAsync(fileFolder, userStorage, cancellationToken);
                        break;
                    case FileFolderType.Folder:
                        await _folderService.DeleteFolderAndDescendantsAsync(userId, fileFolder, userStorage, cancellationToken);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

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

    public async Task MoveToFolderAsync(string userId, Guid itemId, Guid? targetFolderId, CancellationToken cancellationToken = default)
    {
        var item = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == itemId);
        if (item is null)
        {
            throw new ItemNotFoundException(userId, itemId);
        }

        var targetFolder = targetFolderId is null ? null : _dbContext.FileFolders.FirstOrDefault(x => 
            x.UserId == userId && x.Id == targetFolderId && x.Type == FileFolderType.Folder && !x.IsInTrash);
        
        if (targetFolderId is not null && targetFolder is null)
        {
            throw new FolderNotFoundException(userId, targetFolderId.Value);
        }
        
        // Check if trying to move a folder into its own subfolder
        if (targetFolder is not null && item.Type == FileFolderType.Folder && 
            (targetFolder.Path.StartsWith(item.Path + "/") || targetFolder.Id == itemId))
        {
            throw new InvalidOperationException("Cannot move a folder into its own subfolder");
        }

        string oldPath = item.Path;
        
        // Update item's path and parent
        item.ParentId = targetFolderId;
        item.Path = Path.Join(targetFolder?.Path ?? "/", item.Name);
        
        // If it's a folder, update paths of all descendants
        if (item.Type == FileFolderType.Folder)
        {
            var descendants = _dbContext.FileFolders
                .Where(x => x.UserId == userId && x.Path.StartsWith(oldPath + "/"))
                .ToList();
                
            foreach (var descendant in descendants)
            {
                descendant.Path = descendant.Path.Replace(oldPath, item.Path);
            }
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreTrash(string userId, CancellationToken cancellationToken = default)
    {
        var trashItems = _dbContext.FileFolders.Where(x => x.UserId == userId && x.IsInTrash);
        foreach (var item in trashItems)
        {
            item.IsInTrash = false;
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(FileFolderAccessed notification, CancellationToken cancellationToken)
    {
        var fileFolder = _dbContext.FileFolders.First(x => x.Id == notification.FileFolder.Id);
        fileFolder.LastAccessed = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
