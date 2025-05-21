namespace FileFlow.Api.Endpoints.ItemsEndpoints;

public class StarItemEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.ItemsEndpoints.StarItem, (int id) => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(StarItemEndpoint);
}
