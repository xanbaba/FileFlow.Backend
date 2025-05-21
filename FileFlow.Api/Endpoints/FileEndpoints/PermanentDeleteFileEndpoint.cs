namespace FileFlow.Api.Endpoints.FileEndpoints;

public class PermanentDeleteFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete(Contracts.Endpoints.FileEndpoints.PermanentDeleteFile, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(PermanentDeleteFileEndpoint);
}
