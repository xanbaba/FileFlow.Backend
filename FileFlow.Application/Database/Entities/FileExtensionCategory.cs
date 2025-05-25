using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileFlow.Application.Database.Entities;

public class FileExtensionCategory
{
    public int Id { get; set; }
    public string Extension { get; set; } = null!;
    public FileCategory Category { get; set; }
}

internal  class FileExtensionCategoryEntityConfiguration : IEntityTypeConfiguration<FileExtensionCategory>
{
    public void Configure(EntityTypeBuilder<FileExtensionCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Extension).IsUnique();

        builder.Property(x => x.Extension).HasMaxLength(255);

        builder.Property(x => x.Category).HasConversion<string>().HasMaxLength(255);

        builder.HasData(
            new FileExtensionCategory { Id = -1, Extension = ".doc", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -2, Extension = ".docx", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -3, Extension = ".pdf", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -4, Extension = ".txt", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -5, Extension = ".rtf", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -6, Extension = ".odt", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -7, Extension = ".xls", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -8, Extension = ".xlsx", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -9, Extension = ".csv", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -10, Extension = ".ppt", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -11, Extension = ".pptx", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -12, Extension = ".md", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -13, Extension = ".json", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -14, Extension = ".xml", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -15, Extension = ".yaml", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -16, Extension = ".yml", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -17, Extension = ".log", Category = FileCategory.Document },
            new FileExtensionCategory { Id = -18, Extension = ".jpg", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -19, Extension = ".jpeg", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -20, Extension = ".png", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -21, Extension = ".gif", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -22, Extension = ".bmp", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -23, Extension = ".webp", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -24, Extension = ".tiff", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -25, Extension = ".tif", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -26, Extension = ".svg", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -27, Extension = ".heic", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -28, Extension = ".ico", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -29, Extension = ".raw", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -30, Extension = ".psd", Category = FileCategory.Image },
            new FileExtensionCategory { Id = -31, Extension = ".mp4", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -32, Extension = ".mkv", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -33, Extension = ".mov", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -34, Extension = ".avi", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -35, Extension = ".wmv", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -36, Extension = ".flv", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -37, Extension = ".webm", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -38, Extension = ".mpeg", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -39, Extension = ".mpg", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -40, Extension = ".3gp", Category = FileCategory.Video },
            new FileExtensionCategory { Id = -41, Extension = ".m4v", Category = FileCategory.Video }
        );
    }
}