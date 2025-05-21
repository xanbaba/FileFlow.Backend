namespace FileFlow.Api.Endpoints.FileEndpoints;

public class GetFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.GetFile, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetFileEndpoint);
}
