using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.Services.Abstractions;

public interface IUserStorageService
{
    /// <summary>
    /// Creates UserStorage if the specified user does not have one
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Whether UserStorage existed or no</returns>
    public Task<bool> CreateIfNotExistsAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the UserStorage for the specified user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>UserStorage associated with the given user. Null if not found</returns>
    public Task<UserStorage?> GetAsync(string userId, CancellationToken cancellationToken = default);
    
    internal Task UpdateAsync(UserStorage userStorage, CancellationToken cancellationToken = default);
}