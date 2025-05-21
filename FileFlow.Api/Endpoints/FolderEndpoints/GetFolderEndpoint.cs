namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class GetFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FolderEndpoints.GetFolder, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetFolderEndpoint);
}
