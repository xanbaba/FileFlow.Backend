using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.FileStorage;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Services.Exceptions;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Application.Services;

internal class FileService : IFileService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public FileService(AppDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<FileFolder> UploadAsync(string userId, string fileName, string? targetFolderPath, Stream stream,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Guid? parentId = null;
            if (targetFolderPath is not null)
            {
                var parent = _dbContext.FileFolders.FirstOrDefault(x =>
                    x.UserId == userId && x.Path == targetFolderPath);
                if (parent is null)
                    throw new FolderNotFoundException(userId, targetFolderPath);
                parentId = parent.Id;
            }

            var file = new FileFolder
            {
                Id = Guid.CreateVersion7(),
                IsStarred = false,
                IsInTrash = false,
                UserId = userId,
                // ToDo: Validate file name before uploading
                Name = fileName,
                Path = Path.Join(targetFolderPath ?? string.Empty, fileName),
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

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _fileStorage.CommitAsync();
            return file;
        }
        catch (Exception)
        {
            await _fileStorage.RollbackTransaction();
            throw;
        }
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

        // ToDo: Validate new name before renaming
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

    public Task DeletePermanentlyAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);
        _dbContext.FileFolders.Remove(file);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreFromTrashAsync(string userId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

        file.IsInTrash = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}