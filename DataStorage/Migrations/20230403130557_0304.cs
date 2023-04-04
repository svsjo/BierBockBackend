using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorage.Migrations
{
    /// <inheritdoc />
    public partial class _0304 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FavouriteBeerCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "BeerCode",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "BeerCode",
                table: "ChallengeParts");

            migrationBuilder.AddColumn<int>(
                name: "BeerId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "DrinkActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DrinkActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeerId",
                table: "ChallengeParts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Altitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BeerId",
                table: "Users",
                column: "BeerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkActions_LocationId",
                table: "DrinkActions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkActions_ProductId",
                table: "DrinkActions",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeParts_BeerId",
                table: "ChallengeParts",
                column: "BeerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeParts_Products_BeerId",
                table: "ChallengeParts",
                column: "BeerId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkActions_Coordinate_LocationId",
                table: "DrinkActions",
                column: "LocationId",
                principalTable: "Coordinate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkActions_Products_ProductId",
                table: "DrinkActions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Coordinate_LocationId",
                table: "Users",
                column: "LocationId",
                principalTable: "Coordinate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Products_BeerId",
                table: "Users",
                column: "BeerId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeParts_Products_BeerId",
                table: "ChallengeParts");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkActions_Coordinate_LocationId",
                table: "DrinkActions");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkActions_Products_ProductId",
                table: "DrinkActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Coordinate_LocationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Products_BeerId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropIndex(
                name: "IX_Users_BeerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LocationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_DrinkActions_LocationId",
                table: "DrinkActions");

            migrationBuilder.DropIndex(
                name: "IX_DrinkActions_ProductId",
                table: "DrinkActions");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeParts_BeerId",
                table: "ChallengeParts");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DrinkActions");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "ChallengeParts");

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FavouriteBeerCode",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "DrinkActions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BeerCode",
                table: "DrinkActions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "DrinkActions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "DrinkActions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BeerCode",
                table: "ChallengeParts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
