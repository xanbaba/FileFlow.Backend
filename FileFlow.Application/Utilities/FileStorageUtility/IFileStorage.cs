namespace FileFlow.Application.Utilities.FileStorageUtility;

internal interface IFileStorage
{
    public Task<long> UploadFileAsync(Guid fileId, Stream stream);
    public Task<Stream> DownloadFileAsync(Guid fileId);
    public Task DeleteFileIfExistsAsync(Guid fileId);
}