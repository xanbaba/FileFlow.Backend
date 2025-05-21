namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class RestoreFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FolderEndpoints.RestoreFolder, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(RestoreFolderEndpoint);
}
