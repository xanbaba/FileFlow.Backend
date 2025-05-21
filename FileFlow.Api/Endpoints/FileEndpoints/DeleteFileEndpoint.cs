namespace FileFlow.Api.Endpoints.FileEndpoints;

public class DeleteFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FileEndpoints.DeleteFile, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(DeleteFileEndpoint);
}
