using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorage.Migrations
{
    /// <inheritdoc />
    public partial class Nameaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brands = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenericName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutriscoreGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnergyKcalPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FatPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SaturatedFatPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CarbohydratesPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SugarsPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FiberPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProteinsPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SaltPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SodiumPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AlcoholByVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BrewingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fermentation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBU = table.Column<int>(type: "int", nullable: true),
                    Hops = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yeast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Malt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeerStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarbonationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Taste = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Wohnort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Coordinate_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Coordinate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Products_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Challenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PossiblePoints = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NeededQuantity = table.Column<int>(type: "int", nullable: false),
                    SearchString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChallengeType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenge_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DrinkActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrinkActions_Coordinate_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Coordinate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkActions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DrinkActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenge_UserId",
                table: "Challenge",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkActions_LocationId",
                table: "DrinkActions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkActions_ProductId",
                table: "DrinkActions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkActions_UserId",
                table: "DrinkActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BeerId",
                table: "Users",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Challenge");

            migrationBuilder.DropTable(
                name: "DrinkActions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
