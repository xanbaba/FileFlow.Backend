using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace FileFlow.Application.Utilities.EmailUtility;

internal class SupportEmail : ISupportEmail
{
    private readonly EmailSettings _settings;

    public SupportEmail(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendContactSupportMessageAsync(string userEmail, string subject, string message)
    {
        using var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
        smtpClient.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword);
        smtpClient.EnableSsl = true;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = $"Message from: {userEmail}\n\n{message}",
            IsBodyHtml = false
        };

        // Email will be sent to your own support inbox
        mailMessage.To.Add(_settings.SenderEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}