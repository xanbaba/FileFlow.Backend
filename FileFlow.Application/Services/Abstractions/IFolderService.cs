using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Abstractions;

public interface IFolderService
{
    public Task<FileFolder> CreateAsync(string userId, string folderName, Guid? targetFolderId,
        CancellationToken cancellationToken = default);

    public Task<FileFolder> GetMetadataAsync(string userId, Guid folderId,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<FileFolder>> GetChildrenAsync(string userId, Guid? folderId,
        CancellationToken cancellationToken = default);

    public Task RenameAsync(string userId, Guid folderId, string newFolderName,
        CancellationToken cancellationToken = default);

    public Task MoveToTrashAsync(string userId, Guid folderId, CancellationToken cancellationToken = default);

    public Task DeletePermanentlyAsync(string userId, Guid folderId, CancellationToken cancellationToken = default);

    public Task RestoreFromTrashAsync(string userId, Guid folderId, CancellationToken cancellationToken = default);
}