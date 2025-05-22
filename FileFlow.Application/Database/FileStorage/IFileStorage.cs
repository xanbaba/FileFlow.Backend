namespace FileFlow.Application.Database.FileStorage;

internal interface IFileStorage
{
    public Task StartTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransaction();
    public Task UploadFileAsync(Guid fileId, Stream stream);
    public Task<Stream> DownloadFileAsync(Guid fileId);
}