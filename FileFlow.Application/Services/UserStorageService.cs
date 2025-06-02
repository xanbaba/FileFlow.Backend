using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;

namespace FileFlow.Application.Services;

public class UserStorageService : IUserStorageService, IEventHandler<FileUploadedEvent>, IEventHandler<FilePermanentlyDeletedEvent>
{
    private readonly AppDbContext _dbContext;

    public UserStorageService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<bool> CreateIfNotExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (_dbContext.UserStorages.Any(x => x.UserId == userId)) 
            return Task.FromResult(true);
        
        _dbContext.UserStorages.Add(new UserStorage
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            MaxSpace = 10 * 1000, // 10 GB
            UsedSpace = 0
        });
        _dbContext.SaveChanges();
        return Task.FromResult(false);

    }

    public Task<UserStorage?> GetAsync(string userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dbContext.UserStorages.FirstOrDefault(x => x.UserId == userId));
    }

    public Task Handle(FileUploadedEvent notification, CancellationToken cancellationToken)
    {
        var file = notification.File;
        var userStorage = _dbContext.UserStorages.First(x => x.UserId == file.UserId);
        userStorage.UsedSpace += file.Size!.Value;
        switch (file.FileCategory!)
        {
            case FileCategory.Document:
                userStorage.Documents += file.Size.Value;
                break;
            case FileCategory.Image:
                userStorage.Images += file.Size.Value;
                break;
            case FileCategory.Video:
                userStorage.Videos += file.Size.Value;
                break;
            case FileCategory.Other:
                userStorage.Other += file.Size.Value;
                break;
        }
        
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task Handle(FilePermanentlyDeletedEvent notification, CancellationToken cancellationToken)
    {
        var userStorage = _dbContext.UserStorages.First(x => x.UserId == notification.File.UserId);
        userStorage.UsedSpace -= notification.File.Size!.Value;
        switch (notification.File.FileCategory!)
        {
            case FileCategory.Document:
                userStorage.Documents -= notification.File.Size.Value;
                break;
            case FileCategory.Image:
                userStorage.Images -= notification.File.Size.Value;
                break;
            case FileCategory.Video:
                userStorage.Videos -= notification.File.Size.Value;
                break;
            case FileCategory.Other:
                userStorage.Other -= notification.File.Size.Value;
                break;
        }
        
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
}