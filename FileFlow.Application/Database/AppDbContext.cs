using FileFlow.Application.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Application.Database;

internal class AppDbContext : DbContext
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
    public DbSet<FileFolder> FileFolders { get; set; }
}