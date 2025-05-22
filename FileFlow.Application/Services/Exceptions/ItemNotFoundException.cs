namespace FileFlow.Application.Services.Exceptions;

public class ItemNotFoundException : Exception
{
    public string UserId { get; }
    public Guid ItemId { get; }

    public ItemNotFoundException(string userId, Guid itemId)
        : base($"Item with ID {itemId} not found for user {userId}")
    {
        UserId = userId;
        ItemId = itemId;
    }
}
