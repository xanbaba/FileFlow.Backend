using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Abstractions;

public interface IFileService
{
    public Task<FileFolder> UploadAsync(string userId, string fileName, string? targetFolderPath, Stream stream, CancellationToken cancellationToken = default);

    public Task<FileFolder> GetMetadataAsync(string userId, Guid fileId,
        CancellationToken cancellationToken = default);

    public Task<Stream> GetContentAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task RenameAsync(string userId, Guid fileId, string newFileName,
        CancellationToken cancellationToken = default);

    public Task MoveToTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task DeletePermanentlyAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task RestoreFromTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);
}