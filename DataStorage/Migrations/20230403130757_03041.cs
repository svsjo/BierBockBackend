using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorage.Migrations
{
    /// <inheritdoc />
    public partial class _03041 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Products_BeerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BeerId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "FavouriteBeerId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FavouriteBeerId",
                table: "Users",
                column: "FavouriteBeerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Products_FavouriteBeerId",
                table: "Users",
                column: "FavouriteBeerId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Products_FavouriteBeerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FavouriteBeerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FavouriteBeerId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BeerId",
                table: "Users",
                column: "BeerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Products_BeerId",
                table: "Users",
                column: "BeerId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
