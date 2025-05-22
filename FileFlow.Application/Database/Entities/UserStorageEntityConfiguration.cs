using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileFlow.Application.Database.Entities;

internal class UserStorageEntityConfiguration : IEntityTypeConfiguration<UserStorage>
{
    public void Configure(EntityTypeBuilder<UserStorage> builder)
    {
        // Key
        builder.HasKey(x => x.Id);

        // Indices
        builder.HasIndex(x => x.UserId)
            .IsUnique();
        
        // Properties
        builder.Property(x => x.UserId)
            .HasMaxLength(255);
    }
}