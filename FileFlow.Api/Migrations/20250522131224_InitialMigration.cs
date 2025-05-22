using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FileFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileExtensionCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Extension = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileExtensionCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsStarred = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: true),
                    FileCategory = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsInTrash = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStorages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaxSpace = table.Column<int>(type: "int", nullable: false),
                    UsedSpace = table.Column<int>(type: "int", nullable: false),
                    Documents = table.Column<int>(type: "int", nullable: false),
                    Images = table.Column<int>(type: "int", nullable: false),
                    Videos = table.Column<int>(type: "int", nullable: false),
                    Other = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStorages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FileExtensionCategory",
                columns: new[] { "Id", "Category", "Extension" },
                values: new object[,]
                {
                    { -41, 2, ".m4v" },
                    { -40, 2, ".3gp" },
                    { -39, 2, ".mpg" },
                    { -38, 2, ".mpeg" },
                    { -37, 2, ".webm" },
                    { -36, 2, ".flv" },
                    { -35, 2, ".wmv" },
                    { -34, 2, ".avi" },
                    { -33, 2, ".mov" },
                    { -32, 2, ".mkv" },
                    { -31, 2, ".mp4" },
                    { -30, 1, ".psd" },
                    { -29, 1, ".raw" },
                    { -28, 1, ".ico" },
                    { -27, 1, ".heic" },
                    { -26, 1, ".svg" },
                    { -25, 1, ".tif" },
                    { -24, 1, ".tiff" },
                    { -23, 1, ".webp" },
                    { -22, 1, ".bmp" },
                    { -21, 1, ".gif" },
                    { -20, 1, ".png" },
                    { -19, 1, ".jpeg" },
                    { -18, 1, ".jpg" },
                    { -17, 0, ".log" },
                    { -16, 0, ".yml" },
                    { -15, 0, ".yaml" },
                    { -14, 0, ".xml" },
                    { -13, 0, ".json" },
                    { -12, 0, ".md" },
                    { -11, 0, ".pptx" },
                    { -10, 0, ".ppt" },
                    { -9, 0, ".csv" },
                    { -8, 0, ".xlsx" },
                    { -7, 0, ".xls" },
                    { -6, 0, ".odt" },
                    { -5, 0, ".rtf" },
                    { -4, 0, ".txt" },
                    { -3, 0, ".pdf" },
                    { -2, 0, ".docx" },
                    { -1, 0, ".doc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileExtensionCategory_Extension",
                table: "FileExtensionCategory",
                column: "Extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_IsInTrash",
                table: "FileFolders",
                column: "IsInTrash");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_IsStarred",
                table: "FileFolders",
                column: "IsStarred");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_Name",
                table: "FileFolders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_ParentId",
                table: "FileFolders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_Path",
                table: "FileFolders",
                column: "Path");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_Type",
                table: "FileFolders",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_UserId",
                table: "FileFolders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStorages_UserId",
                table: "UserStorages",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileExtensionCategory");

            migrationBuilder.DropTable(
                name: "FileFolders");

            migrationBuilder.DropTable(
                name: "UserStorages");
        }
    }
}
