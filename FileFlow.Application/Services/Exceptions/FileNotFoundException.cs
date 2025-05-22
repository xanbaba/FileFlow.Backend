namespace FileFlow.Application.Services.Exceptions;

public class FileNotFoundException(string userId, Guid fileId) : Exception
{
    public string UserId { get; } = userId;
    public Guid FileId { get; } = fileId;
}