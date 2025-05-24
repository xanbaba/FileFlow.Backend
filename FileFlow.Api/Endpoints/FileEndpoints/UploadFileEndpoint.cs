using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UploadFileForm
{
    public required IFormFile File { get; set; } = null!;

    public required string TargetFolderPath { get; set; } = null!;
}

public class UploadFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.UploadFile,
                async ([FromForm] UploadFileForm uploadFileForm, IFileService fileService, ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    var uploadedFile = await fileService.UploadAsync(user.GetUserid(), uploadFileForm.File.FileName,
                        uploadFileForm.TargetFolderPath,
                        uploadFileForm.File.OpenReadStream(), cancellationToken);

                    return Results.Ok(uploadedFile.ToResponse());
                })
            .WithName(Name)
            .RequireAuthorization()
            .Produces<FileFolderResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public string Name => nameof(UploadFileEndpoint);
}