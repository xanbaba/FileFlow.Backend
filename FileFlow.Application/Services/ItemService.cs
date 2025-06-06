using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;

namespace FileFlow.Application.Services;

internal class ItemService : IItemService
{
    private readonly AppDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public ItemService(AppDbContext dbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
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
            .Where(x => x.LastAccessed.HasValue && x.LastAccessed.Value >= DateTime.UtcNow.AddDays(-1));
        
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
        var trashItems = _dbContext.FileFolders
            .Where(x => x.UserId == userId && x.IsInTrash)
            .ToList();
        foreach (var folder in trashItems.Where(x => x.Type == FileFolderType.Folder))
        {
            var descendants = _dbContext.FileFolders.Where(x => x.UserId == userId && x.Path.StartsWith(folder.Path + "/"));
            _dbContext.FileFolders.RemoveRange(descendants);
            foreach (var file in descendants.Where(x => x.Type == FileFolderType.File))
            {
                await _eventBus.PublishAsync(new FilePermanentlyDeletedEvent(file), cancellationToken);
            }
        }

        _dbContext.FileFolders.RemoveRange(trashItems);
        await _dbContext.SaveChangesAsync(cancellationToken);
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
}
