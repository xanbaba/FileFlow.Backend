using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Exceptions;

public class UserStorageOverflowException(FileFolder fileFolder, UserStorage userStorage) : Exception
{
    public FileFolder FileFolder { get; } = fileFolder;
    public UserStorage UserStorage { get; } = userStorage;
}