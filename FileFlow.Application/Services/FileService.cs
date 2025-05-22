using FileFlow.Application.Database;
using FileFlow.Application.Database.Entities;
using FileFlow.Application.Database.FileStorage;
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

    public async Task<IEnumerable<FileFolder>> UploadAsync(FileUpload[] uploadRequests,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _fileStorage.StartTransactionAsync();
            List<FileFolder> files = [];
            foreach (var uploadRequest in uploadRequests)
            {
                Guid? parentId = null;
                if (uploadRequest.TargetFolderPath is not null)
                {
                    var parent = _dbContext.FileFolders.FirstOrDefault(x =>
                        x.UserId == uploadRequest.UserId && x.Path == uploadRequest.TargetFolderPath);
                    if (parent is null)
                        throw new FolderNotFoundException(uploadRequest.UserId, uploadRequest.TargetFolderPath);
                    parentId = parent.Id;
                }

                var file = new FileFolder
                {
                    Id = Guid.CreateVersion7(),
                    IsStarred = false,
                    IsInTrash = false,
                    UserId = uploadRequest.UserId,
                    Name = uploadRequest.FileName,
                    Path = Path.Join(uploadRequest.TargetFolderPath ?? string.Empty, uploadRequest.FileName),
                    Size = (int)(uploadRequest.Stream.Length / (1024.0 * 1024.0)),
                    Type = FileFolderType.File,
                    ParentId = parentId,
                    FileCategory =
                        _dbContext.FileExtensionCategories
                            .FirstOrDefault(x => x.Extension == Path.GetExtension(uploadRequest.FileName))?.Category ??
                        FileCategory.Other
                };

                _dbContext.FileFolders.Add(file);
                files.Add(file);
                await _fileStorage.UploadFileAsync(file.Id, uploadRequest.Stream);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _fileStorage.CommitTransactionAsync();
            return files;
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

        if (file.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }
        
        return _fileStorage.DownloadFileAsync(fileId);
    }

    public async Task RenameAsync(string userId, Guid fileId, string newFileName,
        CancellationToken cancellationToken = default)
    {
        var file = _dbContext.FileFolders.FirstOrDefault(x => x.UserId == userId && x.Id == fileId);
        if (file is null) throw new FileNotFoundException(userId, fileId);

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