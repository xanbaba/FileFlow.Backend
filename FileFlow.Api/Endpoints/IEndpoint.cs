namespace FileFlow.Api.Endpoints;

public interface IEndpoint
{
    public void Map(IEndpointRouteBuilder builder);
    public string Name { get; }
}