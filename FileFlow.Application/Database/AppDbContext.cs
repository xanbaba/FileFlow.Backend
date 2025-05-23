using FileFlow.Application.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileFlow.Application.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyMarker).Assembly);
    }
    
    // Entities
    internal DbSet<UserStorage> UserStorages { get; set; }
    internal DbSet<FileFolder> FileFolders { get; set; }
    internal DbSet<FileExtensionCategory> FileExtensionCategories { get; set; }
}