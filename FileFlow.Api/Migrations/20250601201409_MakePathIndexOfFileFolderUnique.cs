using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakePathIndexOfFileFolderUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileFolders_Path",
                table: "FileFolders");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_Path",
                table: "FileFolders",
                column: "Path",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileFolders_Path",
                table: "FileFolders");

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_Path",
                table: "FileFolders",
                column: "Path");
        }
    }
}
