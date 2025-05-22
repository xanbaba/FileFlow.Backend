namespace FileFlow.Application.Services.Exceptions;

public class FolderNotFoundException : Exception
{
    public FolderNotFoundException(string userId, string path)
    {
        UserId = userId;
        Path = path;
    }

    public FolderNotFoundException(string userId, Guid folderId)
    {
        UserId = userId;
        FolderId = folderId;
    }
    

    public string UserId { get; }
    public string? Path { get; }
    public Guid? FolderId { get; }
}