using FileFlow.Application.Database.Entities;

namespace FileFlow.Application.MessageBus.Events;

public class FilePermanentlyDeletedEvent(FileFolder file) : IEvent
{
    public FileFolder File { get; } = file;
}