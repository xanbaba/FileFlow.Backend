using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.MessageBus;
using FileFlow.Application.MessageBus.Events;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileFlow.Application.Utilities.FileStorageUtility;
using FluentValidation;
using FluentValidation.Results;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Application.Services;

internal class FileService : IFileService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IEventBus _eventBus;

    public FileService(AppDbContext dbContext, IFileStorage fileStorage, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _eventBus = eventBus;
    }

    public async Task<FileFolder> UploadAsync(string userId, string fileName, Guid? targetFolderId, Stream stream,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Guid? parentId = null;
            FileFolder? parent = null;
            if (targetFolderId is not null)
            {
                parent = _dbContext.FileFolders.FirstOrDefault(x =>
                    x.UserId == userId && x.Id == targetFolderId);
                if (parent is null)
                    throw new FolderNotFoundException(userId, targetFolderId.Value);
                parentId = parent.Id;
            }

            var path = Path.Join(parent?.Path ?? string.Empty, fileName);
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
                Id = Guid.CreateVersion7(),
                IsStarred = false,
                IsInTrash = false,
                UserId = userId,
                Name = fileName,
                Path = path,
                Size = (int)(stream.Length / (1024.0 * 1024.0)),
                Type = FileFolderType.File,
                ParentId = parentId,
                FileCategory =
                    _dbContext.FileExtensionCategories
                        .FirstOrDefault(x => x.Extension == Path.GetExtension(fileName))?.Category ??
                    FileCategory.Other
            };

            _dbContext.FileFolders.Add(file);
            await _fileStorage.UploadFileAsync(file.Id, stream);

            // Save changes
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _fileStorage.CommitAsync();
            
            // Publish the event
            await _eventBus.PublishAsync(new FileUploadedEvent(file), cancellationToken);
            return file;
        }
        catch (Exception)
        {
            await _fileStorage.RollbackTransaction();
            throw;
        }
    }

    private bool ValidateFileName(string fileName)
    {
        var invalidFileNameChars = Path.GetInvalidFileNameChars();
        return invalidFileNameChars.Any(fileName.Contains);
    }

    public Task<FileFolder> GetMetadataAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);
        return Task.FromResult(file);
    }

    public Task<Stream> GetContentAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        return _fileStorage.DownloadFileAsync(fileId);
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
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);
        _dbContext.FileFolders.Remove(file);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        // Publish the event
        await _eventBus.PublishAsync(new FilePermanentlyDeletedEvent(file), cancellationToken);
    }

    public async Task RestoreFromTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        file.IsInTrash = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}