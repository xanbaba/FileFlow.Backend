using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileFlow.Application.Utilities.Auth0Utility;
using FileFlow.Application.Utilities.EmailUtility;
using FluentValidation;
using FluentValidation.Results;

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

        if (subject.Length == 0)
        {
            throw new ValidationException([new ValidationFailure(nameof(subject), "Subject cannot be empty")]);
        }
        if (message.Length == 0)
        {
            throw new ValidationException([new ValidationFailure(nameof(message), "Message cannot be empty")]);
        }
        await _supportEmail.SendContactSupportMessageAsync(userEmail, subject, message, cancellationToken);
    }
}