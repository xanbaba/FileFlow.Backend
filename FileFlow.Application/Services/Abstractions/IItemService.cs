using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Abstractions;

public interface IItemService
{
    public Task<IEnumerable<FileFolder>> GetStarredAsync(string userId, CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<FileFolder>> GetRecentAsync(string userId, CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<FileFolder>> GetInTrashAsync(string userId, CancellationToken cancellationToken = default);
    
    public Task StarAsync(string userId, Guid itemId, CancellationToken cancellationToken = default);
    
    public Task UnstarAsync(string userId, Guid itemId, CancellationToken cancellationToken = default);
    
    public Task EmptyTrashAsync(string userId, CancellationToken cancellationToken = default);
    
    public Task MoveToFolderAsync(string userId, Guid itemId, Guid? targetFolderId, CancellationToken cancellationToken = default);

    public Task RestoreTrash(string userId, CancellationToken cancellationToken = default);
}