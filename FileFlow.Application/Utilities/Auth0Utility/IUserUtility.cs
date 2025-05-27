namespace FileFlow.Application.Utilities.Auth0Utility;

internal interface IUserUtility
{
    public Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default);
}