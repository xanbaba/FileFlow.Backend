namespace FileFlow.Application.Utilities.Auth0Utility;

internal interface IAuth0AccessTokenProvider
{
    public Task<string> GetAccessTokenAsync();
    public Task RefreshAccessTokenAsync();
}