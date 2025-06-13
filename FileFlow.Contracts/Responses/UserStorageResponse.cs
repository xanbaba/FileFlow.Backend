namespace FileFlow.Contracts.Responses;

public class UserStorageResponse
{
    public Guid Id { get; set; }
    public double MaxSpace { get; set; } // MB
    public double UsedSpace { get; set; } // MB

    // Storage breakdown
    public double Documents { get; set; } // MB
    public double Images { get; set; } // MB
    public double Videos { get; set; } // MB
    public double Other { get; set; } // MB
}