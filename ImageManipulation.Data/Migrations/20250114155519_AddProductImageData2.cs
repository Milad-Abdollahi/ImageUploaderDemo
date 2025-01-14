using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageManipulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImageData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Product");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProductImageData",
                table: "Product",
                type: "varbinary(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageData",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
