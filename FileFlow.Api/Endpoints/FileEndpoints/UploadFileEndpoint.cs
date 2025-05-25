using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UploadFileForm
{
    public required IFormFile File { get; set; } = null!;

    public string? TargetFolderPath { get; set; }
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
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(op => new(op)
            {
                Summary = "Uploads a new file",
                Description = "Uploads a new file to the specified folder path.\n\n" +
                              "### Request Form Data\n" +
                              "- **File** (form file): The file to upload\n" +
                              "- **TargetFolderPath** (string, optiona;): The path to the folder where the file should be uploaded. If not specified, root level will be used\n\n" +
                              "### Behavior\n" +
                              "- Validates the uploaded file (size, type, etc.)\n" +
                              "- Checks if the target folder exists and is accessible to the user\n" +
                              "- Stores the file content in the system\n" +
                              "- Creates metadata entry for the file with appropriate file category based on extension\n" +
                              "- If a file with the same name already exists in the target location, may rename the new file automatically\n" +
                              "- Updates user's storage usage statistics\n\n" +
                              "### Response\n" +
                              "Returns a FileFolderResponse object containing metadata about the newly uploaded file, including its assigned ID and path.\n" +
                              "Returns 400 Bad Request if the file is invalid or exceeds user's storage quota.\n" +
                              "Returns 404 Not Found if the target folder doesn't exist."
            });
    }

    public string Name => nameof(UploadFileEndpoint);
}