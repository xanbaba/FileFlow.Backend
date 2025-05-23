using Microsoft.AspNetCore.StaticFiles;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public static class FileEndpointHelpers
{
    
    public static string GetMimeType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            // fallback if unknown
            contentType = "application/octet-stream";
        }
        return contentType;
    }
}