namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class DeleteFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FolderEndpoints.DeleteFolder, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(DeleteFolderEndpoint);
}
