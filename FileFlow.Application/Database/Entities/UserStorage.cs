namespace FileFlow.Application.Database.Entities;

public class UserStorage
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public long MaxSpace { get; set; } // bytes
    public long UsedSpace { get; set; } // bytes

    // Storage breakdown
    public long Documents { get; set; } // bytes
    public long Images { get; set; } // bytes
    public long Videos { get; set; } // bytes
    public long Other { get; set; } // bytes
    
    // Row Version
    public byte[] RowVersion { get; set; } = null!;
}