namespace FileFlow.Api.Endpoints.FileEndpoints;

public class UploadFileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Contracts.Endpoints.FileEndpoints.UploadFile, () => { })
            .WithName(Name)
            .RequireAuthorization();
    }

    public string Name => nameof(UploadFileEndpoint);
}
