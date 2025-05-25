namespace FileFlow.Application.Services.Exceptions;

public class FolderNotFoundException : Exception
{
    public FolderNotFoundException(string userId, string path) : base(
        $"Folder with path \"{path}\" not found for user {userId}")
    {
    }

    public FolderNotFoundException(string userId, Guid folderId) : base(
        $"Folder with ID {folderId} not found for user {userId}")
    {
    }
}