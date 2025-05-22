namespace FileFlow.Api.Endpoints.UserStorageEndpoints;

public class GetStorageEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Contracts.Endpoints.UserEndpoints.GetStorage, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(GetStorageEndpoint);
}