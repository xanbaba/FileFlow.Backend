namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class UpdateFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut(Contracts.Endpoints.FolderEndpoints.UpdateFolder, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(UpdateFolderEndpoint);
}
