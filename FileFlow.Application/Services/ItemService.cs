using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;

namespace FileFlow.Application.Services;

internal class ItemService : IItemService
{
    private readonly AppDbContext _dbContext;

    public ItemService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
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
        // TODO: Add LastAccessed field
        
        // Assuming we want the most recently created/modified items
        // You might need to adjust this query if you have a specific LastAccessed field
        var items = _dbContext.FileFolders
            .Where(x => x.UserId == userId && !x.IsInTrash)
            .OrderByDescending(x => x.Id) // Using Id as a proxy for creation time assuming it uses Guid.NewGuid()
            .Take(20) // Limit to recent 20 items
            .ToList();
        
        return Task.FromResult<IEnumerable<FileFolder>>(items);
    }

    public Task<IEnumerable<FileFolder>> GetInTrashAsync(string userId, CancellationToken cancellationToken = default)
    {
        var items = _dbContext.FileFolders
            .Where(x => x.UserId == userId && x.IsInTrash)
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
}
