namespace FileFlow.Application.Utilities.EmailUtility;

internal interface ISupportEmail
{
    public Task SendContactSupportMessageAsync(string userEmail, string subject, string message);
}