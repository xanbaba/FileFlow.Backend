using FileFlow.Application.Database.Entities;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints;

public static class Mapper
{
    public static FileFolderResponse ToResponse(this FileFolder fileFolder) => new()
    {
        Id = fileFolder.Id,
        Name = fileFolder.Name,
        Type = fileFolder.Type.ToString().ToLower(),
        IsStarred = fileFolder.IsStarred,
        Path = fileFolder.Path
    };

    public static UserStorageResponse ToResponse(this UserStorage userStorage) =>
        new()
        {
            Id = userStorage.Id,
            Documents = userStorage.Documents / (1000.0 * 1000.0),
            Images = userStorage.Images / (1000.0 * 1000.0),
            MaxSpace = userStorage.MaxSpace / (1000.0 * 1000.0),
            Other = userStorage.Other / (1000.0 * 1000.0),
            Videos = userStorage.Videos / (1000.0 * 1000.0),
            UsedSpace = userStorage.UsedSpace / (1000.0 * 1000.0)
        };
}