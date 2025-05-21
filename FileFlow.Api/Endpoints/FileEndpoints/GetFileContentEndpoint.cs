namespace FileFlow.Api.Endpoints.FileEndpoints;

public class GetFileContentEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.FileEndpoints.GetFileContent, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetFileContentEndpoint);
}
