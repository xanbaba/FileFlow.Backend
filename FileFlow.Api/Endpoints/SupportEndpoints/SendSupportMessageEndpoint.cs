using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Contracts.Requests;

namespace FileFlow.Api.Endpoints.SupportEndpoints;

public class SendSupportMessageEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.SupportEndpoints.SendSupportMessage,
            async (SendSupportMessageRequest request, ISupportService supportService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                await supportService.SendSupportMessageAsync(user.GetUserid(), request.Subject, request.Message, cancellationToken);
                return Results.Ok();
            })
            .WithName(Name)
            .RequireAuthorization()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public string Name => nameof(SendSupportMessageEndpoint);
}