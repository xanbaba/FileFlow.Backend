namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class PermanentDeleteFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FolderEndpoints.PermanentDeleteFolder, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(PermanentDeleteFolderEndpoint);
}
