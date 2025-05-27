namespace FileFlow.Contracts.Requests;

public class SendSupportMessageRequest
{
    public required string Subject { get; set; }
    public required string Message { get; set; }
}