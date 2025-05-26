namespace FileFlow.Application.Utilities.EmailUtility;

internal class EmailSettings
{
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SenderEmail { get; set; }
    public required string SenderName { get; set; }
    public required string SenderPassword { get; set; }
}