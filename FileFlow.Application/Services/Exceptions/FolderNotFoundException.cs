namespace FileFlow.Application.Services.Exceptions;

public class FolderNotFoundException(string userId, string path) : Exception
{
    public string UserId { get; } = userId;
    public string Path { get; } = path;
}