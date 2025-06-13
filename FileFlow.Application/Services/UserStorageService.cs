using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

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
            MaxSpace = 10_000_000_000, // 10 GB
            UsedSpace = 0
        });
        try
        {
            _dbContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            // ignore
        }
        return Task.FromResult(false);

    }

    public Task<UserStorage?> GetAsync(string userId, CancellationToken cancellationToken = default)
    {
        var userStorage = _dbContext.UserStorages.FirstOrDefault(x => x.UserId == userId);
        return Task.FromResult(userStorage);
    }

    public Task Handle(FileUploadedEvent notification, CancellationToken cancellationToken)
    {
        return UpdateUserStorage(notification.File, false, cancellationToken);
    }

    public Task Handle(FilePermanentlyDeletedEvent notification, CancellationToken cancellationToken)
    {
        return UpdateUserStorage(notification.File, true, cancellationToken);
    }

    private const int MaxRetries = 3;
    
    private async Task UpdateUserStorage(FileFolder file, bool isDelete, CancellationToken cancellationToken = default)
    {
        
        long change = isDelete ? -file.Size!.Value : file.Size!.Value;
        for (int i = 0; i < MaxRetries; i++)
        {
            try
            {
                var userStorage = _dbContext.UserStorages.First(x => x.UserId == file.UserId);
                userStorage.UsedSpace += change;
                
                switch (file.FileCategory!)
                {
                    case FileCategory.Document:
                        userStorage.Documents += change;
                        break;
                    case FileCategory.Image:
                        userStorage.Images += change;
                        break;
                    case FileCategory.Video:
                        userStorage.Videos += change;
                        break;
                    default:
                        userStorage.Other += change;
                        break;
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                var entityEntry = e.Entries.Single();
                await entityEntry.ReloadAsync(cancellationToken);

                if (i == MaxRetries - 1)
                {
                    throw;
                }
            }
        }
    }
}