namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UpdateFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut(Contracts.Endpoints.FileEndpoints.UpdateFile, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(UpdateFileEndpoint);
}
