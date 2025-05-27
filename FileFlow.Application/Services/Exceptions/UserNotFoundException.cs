namespace FileFlow.Application.Services.Exceptions;

public class UserNotFoundException(string userId) : Exception($"User not found with ID {userId}");