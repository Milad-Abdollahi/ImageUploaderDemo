using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageManipulation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatorUserToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatorUserId",
                table: "Product",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Users_CreatorUserId",
                table: "Product",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Users_CreatorUserId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CreatorUserId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Product");
        }
    }
}
