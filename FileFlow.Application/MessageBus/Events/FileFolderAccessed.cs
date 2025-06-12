using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.MessageBus.Events;

public class FileFolderAccessed(FileFolder fileFolder) : IEvent
{
    public FileFolder FileFolder => fileFolder;
}