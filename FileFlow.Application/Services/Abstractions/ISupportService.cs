namespace FileFlow.Application.Services.Abstractions;

public interface ISupportService
{
    public Task SendSupportMessageAsync(string userId, string subject, string message, CancellationToken cancellationToken = default);
}