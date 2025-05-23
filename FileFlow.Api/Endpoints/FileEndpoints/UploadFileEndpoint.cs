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
                    
                    return Results.Ok(uploadedFile.ToResponse());
                    
                })
            .WithName(Name)
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public string Name => nameof(UploadFileEndpoint);
}
