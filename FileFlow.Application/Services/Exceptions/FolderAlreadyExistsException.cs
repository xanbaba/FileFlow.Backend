namespace FileFlow.Application.Services.Exceptions;

public class FolderAlreadyExistsException(string userId, string path)
    : Exception($"Folder with path \"{path}\" already exists for user {userId}");