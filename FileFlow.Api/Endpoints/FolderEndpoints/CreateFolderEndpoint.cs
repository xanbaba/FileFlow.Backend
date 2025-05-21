namespace FileFlow.Api.Endpoints.FolderEndpoints;

public class CreateFolderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FolderEndpoints.CreateFolder, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(CreateFolderEndpoint);
}
