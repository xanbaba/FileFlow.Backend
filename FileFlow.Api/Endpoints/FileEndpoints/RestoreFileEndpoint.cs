namespace FileFlow.Api.Endpoints.FileEndpoints;

public class RestoreFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.RestoreFile, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(RestoreFileEndpoint);
}
