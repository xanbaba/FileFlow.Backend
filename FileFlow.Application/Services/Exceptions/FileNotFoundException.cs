namespace FileFlow.Application.Services.Exceptions;

public class FileNotFoundException(string? userId, Guid? fileId)
    : Exception($"File with ID {fileId} not found for user {userId}");