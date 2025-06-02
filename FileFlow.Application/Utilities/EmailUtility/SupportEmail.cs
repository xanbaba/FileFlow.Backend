using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace FileFlow.Application.Utilities.EmailUtility;

internal class SupportEmail : ISupportEmail
{
    private readonly MailjetSettings _settings;

    public SupportEmail(IOptions<MailjetSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendContactSupportMessageAsync(string userEmail, string subject, string message,
        CancellationToken cancellationToken = default)
    {
        MailjetClient client = new MailjetClient(_settings.ApiKey, _settings.SecretKey);
        MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
            .Property(Send.Messages, new JArray
            {
                new JObject
                {
                    {
                        "From", new JObject
                        {
                            { "Email", _settings.SenderEmail },
                            { "Name", "File Flow Support" }
                        }
                    },
                    {
                        "To", new JArray
                        {
                            new JObject
                            {
                                { "Email", _settings.SenderEmail },
                                { "Name", "You" }
                            }
                        }
                    },
                    { "Subject", $"File Flow Support: {subject}" },
                    {
                        "TextPart",
                        $"You have received a new support message from {userEmail}.\n\n" +
                        $"Subject: {subject}\n\n" +
                        $"Message:\n{message}\n\n" +
                        $"Reply to: {userEmail}\n\n" +
                        $"--\nFile Flow Support Team"
                    },
                    {
                        "HTMLPart",
                        $"<html>" +
                        $"<body style=\"font-family: Arial, sans-serif; font-size: 14px; color: #333;\">" +
                        $"<h2 style=\"color: #0056b3;\">New Support Message</h2>" +
                        $"<p><strong>From:</strong> <a href=\"mailto:{userEmail}\" style=\"color: #0056b3;\">{userEmail}</a></p>" +
                        $"<p><strong>Subject:</strong> {System.Net.WebUtility.HtmlEncode(subject)}</p>" +
                        $"<p><strong>Message:</strong></p>" +
                        $"<div style=\"background-color: #f9f9f9; padding: 10px; border-left: 4px solid #0056b3;\">" +
                        $"{System.Net.WebUtility.HtmlEncode(message).Replace("\n", "<br/>")}" +
                        $"</div>" +
                        $"<p style=\"margin-top: 20px;\">--<br/>File Flow Support Team</p>" +
                        $"</body></html>"
                    }
                }
            });
        MailjetResponse response = await client.PostAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new MailjetFailedException(response);
        }
    }
}

internal class MailjetFailedException(MailjetResponse response) : Exception(
    $"Status code: {response.StatusCode}\nError info: {response.GetErrorInfo()}\nError message: {response.GetErrorMessage()}");