using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileFlow.Application.Utilities.Auth0Utility;
using FileFlow.Application.Utilities.EmailUtility;

namespace FileFlow.Application.Services;

internal class SupportService : ISupportService
{
    private readonly IUserUtility _userUtility;
    private readonly ISupportEmail _supportEmail;

    public SupportService(IUserUtility userUtility, ISupportEmail supportEmail)
    {
        _userUtility = userUtility;
        _supportEmail = supportEmail;
    }

    public async Task SendSupportMessageAsync(string userId, string subject, string message,
        CancellationToken cancellationToken = default)
    {
        var userEmail = await _userUtility.GetUserEmailAsync(userId, cancellationToken);
        if (userEmail is null)
        {
            throw new UserNotFoundException(userId);
        }

        // ToDo: Validate subject and message before sending email
        await _supportEmail.SendContactSupportMessageAsync(userEmail, subject, message, cancellationToken);
    }
}