﻿// <auto-generated />
using System;
using FileFlow.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileFlow.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250613171402_MakeFileSizesExact")]
    partial class MakeFileSizesExact
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FileFlow.Application.Database.Entities.FileExtensionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Extension")
                        .IsUnique();

                    b.ToTable("FileExtensionCategories");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Category = "Document",
                            Extension = ".doc"
                        },
                        new
                        {
                            Id = -2,
                            Category = "Document",
                            Extension = ".docx"
                        },
                        new
                        {
                            Id = -3,
                            Category = "Document",
                            Extension = ".pdf"
                        },
                        new
                        {
                            Id = -4,
                            Category = "Document",
                            Extension = ".txt"
                        },
                        new
                        {
                            Id = -5,
                            Category = "Document",
                            Extension = ".rtf"
                        },
                        new
                        {
                            Id = -6,
                            Category = "Document",
                            Extension = ".odt"
                        },
                        new
                        {
                            Id = -7,
                            Category = "Document",
                            Extension = ".xls"
                        },
                        new
                        {
                            Id = -8,
                            Category = "Document",
                            Extension = ".xlsx"
                        },
                        new
                        {
                            Id = -9,
                            Category = "Document",
                            Extension = ".csv"
                        },
                        new
                        {
                            Id = -10,
                            Category = "Document",
                            Extension = ".ppt"
                        },
                        new
                        {
                            Id = -11,
                            Category = "Document",
                            Extension = ".pptx"
                        },
                        new
                        {
                            Id = -12,
                            Category = "Document",
                            Extension = ".md"
                        },
                        new
                        {
                            Id = -13,
                            Category = "Document",
                            Extension = ".json"
                        },
                        new
                        {
                            Id = -14,
                            Category = "Document",
                            Extension = ".xml"
                        },
                        new
                        {
                            Id = -15,
                            Category = "Document",
                            Extension = ".yaml"
                        },
                        new
                        {
                            Id = -16,
                            Category = "Document",
                            Extension = ".yml"
                        },
                        new
                        {
                            Id = -17,
                            Category = "Document",
                            Extension = ".log"
                        },
                        new
                        {
                            Id = -18,
                            Category = "Image",
                            Extension = ".jpg"
                        },
                        new
                        {
                            Id = -19,
                            Category = "Image",
                            Extension = ".jpeg"
                        },
                        new
                        {
                            Id = -20,
                            Category = "Image",
                            Extension = ".png"
                        },
                        new
                        {
                            Id = -21,
                            Category = "Image",
                            Extension = ".gif"
                        },
                        new
                        {
                            Id = -22,
                            Category = "Image",
                            Extension = ".bmp"
                        },
                        new
                        {
                            Id = -23,
                            Category = "Image",
                            Extension = ".webp"
                        },
                        new
                        {
                            Id = -24,
                            Category = "Image",
                            Extension = ".tiff"
                        },
                        new
                        {
                            Id = -25,
                            Category = "Image",
                            Extension = ".tif"
                        },
                        new
                        {
                            Id = -26,
                            Category = "Image",
                            Extension = ".svg"
                        },
                        new
                        {
                            Id = -27,
                            Category = "Image",
                            Extension = ".heic"
                        },
                        new
                        {
                            Id = -28,
                            Category = "Image",
                            Extension = ".ico"
                        },
                        new
                        {
                            Id = -29,
                            Category = "Image",
                            Extension = ".raw"
                        },
                        new
                        {
                            Id = -30,
                            Category = "Image",
                            Extension = ".psd"
                        },
                        new
                        {
                            Id = -31,
                            Category = "Video",
                            Extension = ".mp4"
                        },
                        new
                        {
                            Id = -32,
                            Category = "Video",
                            Extension = ".mkv"
                        },
                        new
                        {
                            Id = -33,
                            Category = "Video",
                            Extension = ".mov"
                        },
                        new
                        {
                            Id = -34,
                            Category = "Video",
                            Extension = ".avi"
                        },
                        new
                        {
                            Id = -35,
                            Category = "Video",
                            Extension = ".wmv"
                        },
                        new
                        {
                            Id = -36,
                            Category = "Video",
                            Extension = ".flv"
                        },
                        new
                        {
                            Id = -37,
                            Category = "Video",
                            Extension = ".webm"
                        },
                        new
                        {
                            Id = -38,
                            Category = "Video",
                            Extension = ".mpeg"
                        },
                        new
                        {
                            Id = -39,
                            Category = "Video",
                            Extension = ".mpg"
                        },
                        new
                        {
                            Id = -40,
                            Category = "Video",
                            Extension = ".3gp"
                        },
                        new
                        {
                            Id = -41,
                            Category = "Video",
                            Extension = ".m4v"
                        });
                });

            modelBuilder.Entity("FileFlow.Application.Database.Entities.FileFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileCategory")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsInTrash")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStarred")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastAccessed")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("IsInTrash");

                    b.HasIndex("IsStarred");

                    b.HasIndex("LastAccessed");

                    b.HasIndex("Name");

                    b.HasIndex("ParentId");

                    b.HasIndex("Path")
                        .IsUnique();

                    b.HasIndex("Type");

                    b.HasIndex("UserId");

                    b.ToTable("FileFolders");
                });

            modelBuilder.Entity("FileFlow.Application.Database.Entities.UserStorage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Documents")
                        .HasColumnType("bigint");

                    b.Property<long>("Images")
                        .HasColumnType("bigint");

                    b.Property<long>("MaxSpace")
                        .HasColumnType("bigint");

                    b.Property<long>("Other")
                        .HasColumnType("bigint");

                    b.Property<long>("UsedSpace")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("Videos")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserStorages");
                });

            modelBuilder.Entity("FileFlow.Application.Database.Entities.FileFolder", b =>
                {
                    b.HasOne("FileFlow.Application.Database.Entities.FileFolder", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });
#pragma warning restore 612, 618
        }
    }
}
