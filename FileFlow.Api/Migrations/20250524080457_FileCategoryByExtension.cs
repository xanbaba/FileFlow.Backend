using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class FileCategoryByExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileExtensionCategory",
                table: "FileExtensionCategory");

            migrationBuilder.RenameTable(
                name: "FileExtensionCategory",
                newName: "FileExtensionCategories");

            migrationBuilder.RenameIndex(
                name: "IX_FileExtensionCategory_Extension",
                table: "FileExtensionCategories",
                newName: "IX_FileExtensionCategories_Extension");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "FileExtensionCategories",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "FileExtensionCategories",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileExtensionCategories",
                table: "FileExtensionCategories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -41,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -40,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -39,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -38,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -37,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -36,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -35,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -34,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -33,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -32,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -31,
                column: "Category",
                value: "Video");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -30,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -29,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -28,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -27,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -26,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -25,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -24,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -23,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -22,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -21,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -20,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -19,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -18,
                column: "Category",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -17,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -16,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -15,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -14,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -13,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -12,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -11,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -10,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -9,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -8,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -7,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -6,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -5,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -4,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -3,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -2,
                column: "Category",
                value: "Document");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategories",
                keyColumn: "Id",
                keyValue: -1,
                column: "Category",
                value: "Document");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileExtensionCategories",
                table: "FileExtensionCategories");

            migrationBuilder.RenameTable(
                name: "FileExtensionCategories",
                newName: "FileExtensionCategory");

            migrationBuilder.RenameIndex(
                name: "IX_FileExtensionCategories_Extension",
                table: "FileExtensionCategory",
                newName: "IX_FileExtensionCategory_Extension");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "FileExtensionCategory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "FileExtensionCategory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileExtensionCategory",
                table: "FileExtensionCategory",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -41,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -40,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -39,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -38,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -37,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -36,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -35,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -34,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -33,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -32,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -31,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -30,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -29,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -28,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -27,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -26,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -25,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -24,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -23,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -22,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -21,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -20,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -19,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -18,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -17,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -16,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -15,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -14,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -13,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -12,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -11,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -10,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -9,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -8,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -7,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -6,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -5,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -4,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -3,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -2,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "FileExtensionCategory",
                keyColumn: "Id",
                keyValue: -1,
                column: "Category",
                value: 0);
        }
    }
}
