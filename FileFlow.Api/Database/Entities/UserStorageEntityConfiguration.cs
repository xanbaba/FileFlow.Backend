using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileFlow.Api.Database.Entities;

public class UserStorageEntityConfiguration : IEntityTypeConfiguration<UserStorage>
{
    public void Configure(EntityTypeBuilder<UserStorage> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserId)
            .HasMaxLength(100)
            .IsRequired();
    }
}