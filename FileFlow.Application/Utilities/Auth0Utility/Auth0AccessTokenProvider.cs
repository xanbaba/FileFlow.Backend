using System.Text;
using System.Text.Json;
using FileFlow.Application.Options;
using Microsoft.Extensions.Options;

namespace FileFlow.Application.Utilities.Auth0Utility;

internal class Auth0AccessTokenProvider : IAuth0AccessTokenProvider
{
    private readonly IOptions<Auth0Options> _options;
    private readonly HttpClient _client;

    public Auth0AccessTokenProvider(IOptions<Auth0Options> options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _client = httpClientFactory.CreateClient("Auth0Client");
    }

    private static string? _accessToken;

    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken is null)
        {
            await RefreshAccessTokenAsync();
        }
        return _accessToken!;
    }

    public async Task RefreshAccessTokenAsync()
    {

        var requestBody = new
        {
            client_id = _options.Value.ClientId,
            client_secret = _options.Value.ClientSecret,
            audience = _options.Value.Audience,
            grant_type = "client_credentials"
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/oauth/token", jsonContent);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseContentDeserialized = JsonSerializer.Deserialize<dynamic>(responseContent)!;
        _accessToken = responseContentDeserialized.access_token;
    }
}