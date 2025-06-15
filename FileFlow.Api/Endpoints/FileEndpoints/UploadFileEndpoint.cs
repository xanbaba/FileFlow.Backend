using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileFlow.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UploadFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.UploadFile,
                [RequestSizeLimit(1_099_511_627_776)]
                async ([FromHeader(Name = "X-File-Name")] string fileName,
                    [FromHeader(Name = "X-Target-Folder-Id")] Guid? targetFolderId,
                    [FromHeader(Name = "X-File-Size")] long fileSize,
                    HttpRequest httpRequest,
                    IFileService fileService, ClaimsPrincipal user,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var uploadedFile = await fileService.UploadAsync(user.GetUserid(), fileName, fileSize,
                            targetFolderId, httpRequest.Body, cancellationToken);

                        return Results.Ok(uploadedFile.ToResponse());
                    }
                    catch (UserStorageOverflowException e)
                    {
                        return Results.BadRequest(new ErrorMessage($"File exceeds user's storage quota.\n\n" +
                                                                   $"Max allowed: {e.UserStorage.MaxSpace} MB.\n" +
                                                                   $"Used: {e.UserStorage.UsedSpace} MB.\n" +
                                                                   $"File size: {e.FileFolder.Size}"));
                    }
                })
            .WithName(Name)
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<Stream>("application/octet-stream")
            .Produces<FileFolderResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(op =>
            {
                op.Summary = "Upload a file via raw binary stream";
                op.Description = "The file is sent as a binary stream in the body.";

                // Document the custom header
                op.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
                {
                    Name = "X-File-Name",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Required = true,
                    Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                    {
                        Type = "string"
                    },
                    Description = "The name to save the uploaded file as"
                });
                
                // Document the custom header
                op.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
                {
                    Name = "X-File-Size",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Required = true,
                    Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int64"
                    },
                    Description = "Size of a file in bytes"
                });

                // Document the custom header
                op.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter
                {
                    Name = "X-Target-Folder-Id",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Required = false,
                    Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                    {
                        Type = "string"
                    },
                    Description =
                        "(string, optional): The id of the folder where the file should be uploaded. If not specified, root level will be used"
                });

                // Document the request body
                op.RequestBody = new Microsoft.OpenApi.Models.OpenApiRequestBody
                {
                    Required = true,
                    Content =
                    {
                        ["application/octet-stream"] = new Microsoft.OpenApi.Models.OpenApiMediaType
                        {
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary"
                            }
                        }
                    }
                };

                return op;
            });
    }

    public string Name => nameof(UploadFileEndpoint);
}