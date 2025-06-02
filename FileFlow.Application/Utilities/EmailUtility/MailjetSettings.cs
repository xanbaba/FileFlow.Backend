namespace FileFlow.Application.Utilities.EmailUtility;

internal class MailjetSettings
{
    public required string ApiKey { get; set; }
    public required string SecretKey { get; set; }
    
    public required string SenderEmail { get; set; }
}