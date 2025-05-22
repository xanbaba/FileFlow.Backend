using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Application.FileStorage;

internal class FileStorage : IFileStorage
{
    public FileStorage(IConfiguration configuration)
    {
        _containerClient = new BlobContainerClient(configuration.GetConnectionString("AzureBlobStorage"),
            configuration["AzureBlobContainerName"]);
        
        _containerClient.CreateIfNotExists();
    }
    
    private record FileUpload(Guid FileId, Stream Stream);
    
    private List<FileUpload> _uploads = [];

    private readonly BlobContainerClient _containerClient;

    public async Task CommitAsync()
    {
        foreach (var upload in _uploads)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(upload.FileId.ToString());
            await blobClient.UploadAsync(upload.Stream);
        }

        _uploads = [];
    }

    public Task RollbackTransaction()
    {
        _uploads = [];
        return Task.CompletedTask;
    }

    public Task UploadFileAsync(Guid fileId, Stream stream)
    {
        _uploads.Add(new FileUpload(fileId, stream));
        return Task.CompletedTask;
    }

    public async Task<Stream> DownloadFileAsync(Guid fileId)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileId.ToString());
        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException(null, fileId);
        }
        
        return await blobClient.OpenReadAsync();
    }
}