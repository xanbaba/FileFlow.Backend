namespace FileFlow.Application.FileStorage;

internal interface IFileStorage
{
    public Task CommitAsync();
    public Task RollbackTransaction();
    public Task UploadFileAsync(Guid fileId, Stream stream);
    public Task<Stream> DownloadFileAsync(Guid fileId);
}