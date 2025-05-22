namespace FileFlow.Application.Database.Entities;

public class UserStorage
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public int MaxSpace { get; set; } // MB
    public int UsedSpace { get; set; } // MB

    // Storage breakdown
    public int Documents { get; set; } // MB
    public int Images { get; set; } // MB
    public int Videos { get; set; } // MB
    public int Other { get; set; } // MB
}