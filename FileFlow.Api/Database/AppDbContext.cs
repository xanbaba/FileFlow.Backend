using FileFlow.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    // Entities
    public DbSet<UserStorage> UserStorages { get; set; }
}