using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLastAccessed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "FileFolders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileFolders_LastAccessed",
                table: "FileFolders",
                column: "LastAccessed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileFolders_LastAccessed",
                table: "FileFolders");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "FileFolders");
        }
    }
}
