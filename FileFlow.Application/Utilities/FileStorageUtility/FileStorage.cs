using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Application.Utilities.FileStorageUtility;

internal class FileStorage : IFileStorage
{
    public FileStorage(IConfiguration configuration)
    {
        _containerClient = new BlobContainerClient(configuration.GetConnectionString("AzureBlobStorage"),
            configuration["AzureBlobContainerName"]);
        
        _containerClient.CreateIfNotExists();
    }

    private readonly BlobContainerClient _containerClient;

    public async Task<long> UploadFileAsync(Guid fileId, Stream stream)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileId.ToString());
        await blobClient.UploadAsync(stream);
        return (await blobClient.GetPropertiesAsync()).Value.ContentLength;
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

    public async Task DeleteFileIfExistsAsync(Guid fileId)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileId.ToString());
        await blobClient.DeleteIfExistsAsync();
    }
}