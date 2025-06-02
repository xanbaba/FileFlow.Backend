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
            Documents = userStorage.Documents,
            Images = userStorage.Images,
            MaxSpace = userStorage.MaxSpace,
            Other = userStorage.Other,
            Videos = userStorage.Videos,
            UsedSpace = userStorage.UsedSpace
        };
}