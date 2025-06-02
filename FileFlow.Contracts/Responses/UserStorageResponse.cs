namespace FileFlow.Contracts.Responses;

public class UserStorageResponse
{
    public Guid Id { get; set; }
    public int MaxSpace { get; set; } // MB
    public int UsedSpace { get; set; } // MB

    // Storage breakdown
    public int Documents { get; set; } // MB
    public int Images { get; set; } // MB
    public int Videos { get; set; } // MB
    public int Other { get; set; } // MB
}