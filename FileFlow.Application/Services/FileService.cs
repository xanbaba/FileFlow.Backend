using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileFlow.Application.Utilities.FileStorageUtility;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Application.Services;

internal class FileService : IFileService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IEventBus _eventBus;
    private readonly IUserStorageService _userStorageService;

    public FileService(AppDbContext dbContext, IFileStorage fileStorage, IEventBus eventBus, IUserStorageService userStorageService)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _eventBus = eventBus;
        _userStorageService = userStorageService;
    }

    public async Task<FileFolder> UploadAsync(string userId, string fileName, long fileSize, Guid? targetFolderId,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.CreateVersion7();
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var path = GetUploadPath(userId, fileName, targetFolderId, out var parentId);
            if (_dbContext.FileFolders.Any(x => x.UserId == userId && x.Path == path))
            {
                throw new FileAlreadyExistsException(userId, path);
            }

            if (!ValidateFileName(fileName))
            {
                throw new ValidationException([new ValidationFailure(nameof(fileName), "Invalid file name")]);
            }

            var file = new FileFolder
            {
                Id = fileId,
                IsStarred = false,
                IsInTrash = false,
                UserId = userId,
                Name = fileName,
                Path = path,
                Type = FileFolderType.File,
                Size = fileSize,
                ParentId = parentId,
                FileCategory =
                    _dbContext.FileExtensionCategories
                        .FirstOrDefault(x => x.Extension == Path.GetExtension(fileName))?.Category ??
                    FileCategory.Other
            };

            var userStorage = await _dbContext.UserStorages
                .FromSqlRaw("SELECT TOP(1) * FROM UserStorages WITH (UPDLOCK, ROWLOCK) WHERE UserId = {0}", userId)
                .FirstAsync(cancellationToken: cancellationToken);

            if (userStorage.UsedSpace + fileSize > userStorage.MaxSpace)
            {
                throw new UserStorageOverflowException(file, userStorage);
            }

            _dbContext.FileFolders.Add(file);

            var actualFileSize = await _fileStorage.UploadFileAsync(file.Id, stream);

            if (actualFileSize > fileSize)
            {
                // Delete if the client lied
                await transaction.RollbackAsync(cancellationToken);
                throw new UserStorageOverflowException(file, userStorage);
            }

            if (actualFileSize != fileSize)
            {
                file.Size = actualFileSize;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _userStorageService.UpdateAsync(userStorage, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            // Publish event
            await _eventBus.PublishAsync(new FileFolderAccessed(file), cancellationToken);


            return file;
        }
        catch (Exception)
        {
            await _fileStorage.DeleteFileIfExistsAsync(fileId);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private string GetUploadPath(string userId, string fileName, Guid? targetFolderId, out Guid? parentId)
    {
        parentId = null;
        FileFolder? parent = null;
        if (targetFolderId is not null)
        {
            parent = _dbContext.FileFolders.FirstOrDefault(x =>
                x.UserId == userId && x.Id == targetFolderId);
            if (parent is null)
                throw new FolderNotFoundException(userId, targetFolderId.Value);
            parentId = parent.Id;
        }

        var path = Path.Join(parent?.Path ?? "/", fileName);
        return path;
    }

    private static bool ValidateFileName(string fileName)
    {
        var invalidFileNameChars = Path.GetInvalidFileNameChars();
        return !invalidFileNameChars.Any(fileName.Contains);
    }

    public async Task<FileFolder> GetMetadataAsync(string userId, Guid fileId,
        CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);
        await _eventBus.PublishAsync(new FileFolderAccessed(file), cancellationToken);
        return file;
    }

    public async Task<(FileFolder file, Stream stream)> GetContentAsync(string userId, Guid fileId,
        CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId && !x.IsInTrash);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        var fileDownload = await _fileStorage.DownloadFileAsync(fileId);
        await _eventBus.PublishAsync(new FileFolderAccessed(file), cancellationToken);
        return (file, fileDownload);
    }

    public async Task RenameAsync(string userId, Guid fileId, string newFileName,
        CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        if (!ValidateFileName(newFileName))
        {
            throw new ValidationException([new ValidationFailure(nameof(newFileName), "Invalid file name")]);
        }

        file.Name = newFileName;

        file.Path = string.Join('/', file.Path.Split('/').SkipLast(1), newFileName);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(new FileFolderAccessed(file), cancellationToken);
    }

    public async Task MoveToTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        file.IsInTrash = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePermanentlyAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
            if (file is null) throw new FileNotFoundException(userId, fileId);
            
            var userStorage = await _dbContext.UserStorages
                .FromSqlRaw("SELECT * FROM UserStorages WITH (UPDLOCK, ROWLOCK) WHERE UserId = {0}", userId)
                .FirstAsync(cancellationToken: cancellationToken);
            
            await DeleteFileAsync(file, userStorage, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _userStorageService.UpdateAsync(userStorage, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteFileAsync(FileFolder file, UserStorage userStorage, CancellationToken cancellationToken = default)
    {
        _dbContext.FileFolders.Remove(file);

        await _fileStorage.DeleteFileIfExistsAsync(file.Id);
    }

    public async Task RestoreFromTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        file.IsInTrash = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}