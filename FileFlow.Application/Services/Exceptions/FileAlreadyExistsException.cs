namespace FileFlow.Application.Services.Exceptions;

public class FileAlreadyExistsException(string userId, string path)
    : Exception($"File with path \"{path}\" already exists for user {userId}");