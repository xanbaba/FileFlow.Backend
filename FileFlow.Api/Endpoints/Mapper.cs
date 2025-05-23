using FileFlow.Application.Database.Entities;
using FileFlow.Contracts.Responses;

namespace FileFlow.Api.Endpoints;

public static class Mapper
{
    public static FileFolderResponse ToResponse(this FileFolder fileFolder) => new()
    {
        Id = fileFolder.Id,
        UserId = fileFolder.UserId,
        Name = fileFolder.Name,
        Type = fileFolder.Type.ToString().ToLower(),
        IsStarred = fileFolder.IsStarred,
        Path = fileFolder.Path
    };
}