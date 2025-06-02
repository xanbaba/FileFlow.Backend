using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FileFolderRelationshipWithParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_FileFolders_FileFolders_ParentId",
                table: "FileFolders",
                column: "ParentId",
                principalTable: "FileFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileFolders_FileFolders_ParentId",
                table: "FileFolders");
        }
    }
}
