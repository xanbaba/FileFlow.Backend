using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Application.Services;

public class UserStorageService : IUserStorageService
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

    public Task UpdateAsync(UserStorage userStorage, CancellationToken cancellationToken = default)
    {
        var fileFolders = _dbContext.FileFolders.Where(x => x.UserId == userStorage.UserId);
        var documents = fileFolders.Sum(x => x.FileCategory == FileCategory.Document ? x.Size : 0) ?? 0;
        var images = fileFolders.Sum(x => x.FileCategory == FileCategory.Image ? x.Size : 0) ?? 0;
        var videos = fileFolders.Sum(x => x.FileCategory == FileCategory.Video ? x.Size : 0) ?? 0;
        var other = fileFolders.Sum(x => x.FileCategory == FileCategory.Other ? x.Size : 0) ?? 0;
        
        var usedSpace = documents + images + videos + other;
        userStorage.UsedSpace = usedSpace;
        
        userStorage.Documents = documents;
        userStorage.Images = images;
        userStorage.Videos = videos;
        userStorage.Other = other;
        
        return Task.CompletedTask;
    }
}