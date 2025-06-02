using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileFlow.Application.Database.Entities;

internal class FileFolderEntityConfiguration : IEntityTypeConfiguration<FileFolder>
{
    public void Configure(EntityTypeBuilder<FileFolder> builder)
    {
        // Key
        builder.HasKey(x => x.Id);
        
        // Indices
        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.Path)
            .IsUnique();

        builder.HasIndex(x => x.ParentId);

        builder.HasIndex(x => x.Type);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.IsStarred);

        builder.HasIndex(x => x.IsInTrash);
        
        // Properties
        builder.Property(x => x.UserId)
            .HasMaxLength(255);

        builder.Property(x => x.Name)
            .HasMaxLength(255);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(255);

        builder.Property(x => x.Path)
            .HasMaxLength(4000);

        builder.Property(x => x.FileCategory)
            .HasConversion<string>()
            .HasMaxLength(255);
        
        builder.HasOne(f => f.Parent)
            .WithMany()
            .HasForeignKey(f => f.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}