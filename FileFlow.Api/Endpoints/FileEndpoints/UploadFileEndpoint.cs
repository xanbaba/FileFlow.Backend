using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UploadFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.UploadFile,
                async (IFormFile file, UploadFileRequest request, IFileService fileService, ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    var uploadedFile = await fileService.UploadAsync(user.GetUserid(), file.FileName, request.TargetFolderPath,
                        file.OpenReadStream(), cancellationToken);
                    
                    return Results.Ok(new FileFolderResponse
                    {
                        Id = uploadedFile.Id,
                        UserId = uploadedFile.UserId,
                        Name = uploadedFile.Name,
                        Type = uploadedFile.Type.ToString().ToLower(),
                        IsStarred = uploadedFile.IsStarred,
                        Path = uploadedFile.Path
                    });
                    
                })
            .WithName(Name)
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data");
    }

    public string Name => nameof(UploadFileEndpoint);
}
