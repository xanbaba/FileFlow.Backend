using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FileFlow.Application.Utilities.Auth0Utility;

internal class UserUtility : IUserUtility
{
    private readonly IAuth0AccessTokenProvider _auth0AccessTokenProvider;
    private readonly HttpClient _httpClient;
    
    public UserUtility(IHttpClientFactory httpClientFactory, IAuth0AccessTokenProvider auth0AccessTokenProvider)
    {
        _auth0AccessTokenProvider = auth0AccessTokenProvider;
        _httpClient = httpClientFactory.CreateClient("Auth0Client");
    }
    
    public async Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default)
    {
        var response = await GetUserEmailAsync(userId, await _auth0AccessTokenProvider.GetAccessTokenAsync(), cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _auth0AccessTokenProvider.RefreshAccessTokenAsync();
            response = await GetUserEmailAsync(userId, await _auth0AccessTokenProvider.GetAccessTokenAsync(), cancellationToken);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var deserializedContent = JsonSerializer.Deserialize<JsonElement>(content);
        return deserializedContent.GetProperty("email").GetString();
    }

    private async Task<HttpResponseMessage> GetUserEmailAsync(string userId, string accessToken, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/users/{userId}");
        request.Headers.Add("Authorization", $"Bearer {accessToken}");
        request.Headers.Add("Accept", "application/json");
        var response = await _httpClient.SendAsync(request, cancellationToken);
        return response;
    }
}