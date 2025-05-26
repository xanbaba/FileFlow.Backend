namespace FileFlow.Application.Options;

public class Auth0Options
{
    public required string Domain { get; set; }
    public required string Audience { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}