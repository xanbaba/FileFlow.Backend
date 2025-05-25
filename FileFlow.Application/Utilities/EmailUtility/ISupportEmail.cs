namespace FileFlow.Application.Utilities.EmailUtility;

internal interface ISupportEmail
{
    public Task SendContactSupportMessageAsync(string userEmail, string subject, string message);
}

public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SenderEmail { get; set; }
    public required string SenderName { get; set; }
    public required string SenderPassword { get; set; }
}
