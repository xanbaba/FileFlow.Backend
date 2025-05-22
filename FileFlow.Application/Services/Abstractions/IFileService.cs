using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Abstractions;

public record FileUpload(string UserId, string FileName, string? TargetFolder, Stream Stream);

public interface IFileService
{
    /// <summary>
    /// Uploads files to user's storage
    /// </summary>
    /// <param name="uploadRequests">Upload requests</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>A task representing the completion of the upload operation</returns>
    public Task<IEnumerable<FileFolder>> UploadAsync(FileUpload[] uploadRequests, CancellationToken cancellationToken = default);

    public Task<FileFolder> GetMetadataAsync(string userId, Guid fileId,
        CancellationToken cancellationToken = default);

    public Task<Stream> GetContentAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task RenameAsync(string userId, Guid fileId, string newFileName,
        CancellationToken cancellationToken = default);

    public Task MoveToTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task DeletePermanentlyAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);

    public Task RestoreFromTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default);
}