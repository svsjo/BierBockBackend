using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorage.Migrations
{
    /// <inheritdoc />
    public partial class AddNM2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeChallengePart_ChallengePart_PartialChallengesId",
                table: "ChallengeChallengePart");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeChallengePart_Challenge_ChallengesId",
                table: "ChallengeChallengePart");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeUser_Challenge_UserId",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeUser_User_ChallengeId",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkAction_User_UserId",
                table: "DrinkAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrinkAction",
                table: "DrinkAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChallengePart",
                table: "ChallengePart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Challenge",
                table: "Challenge");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "DrinkAction",
                newName: "DrinkActions");

            migrationBuilder.RenameTable(
                name: "ChallengePart",
                newName: "ChallengeParts");

            migrationBuilder.RenameTable(
                name: "Challenge",
                newName: "Challenges");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkAction_UserId",
                table: "DrinkActions",
                newName: "IX_DrinkActions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrinkActions",
                table: "DrinkActions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChallengeParts",
                table: "ChallengeParts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Brands = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Categories = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<string>(type: "TEXT", nullable: true),
                    GenericName = table.Column<string>(type: "TEXT", nullable: true),
                    IngredientsText = table.Column<string>(type: "TEXT", nullable: true),
                    NutriscoreGrade = table.Column<string>(type: "TEXT", nullable: true),
                    EnergyKcalPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    FatPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    SaturatedFatPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    CarbohydratesPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    SugarsPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    FiberPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    ProteinsPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    SaltPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    SodiumPer100g = table.Column<decimal>(type: "TEXT", nullable: true),
                    AlcoholByVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    BrewingMethod = table.Column<string>(type: "TEXT", nullable: true),
                    Fermentation = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    IBU = table.Column<int>(type: "INTEGER", nullable: true),
                    Hops = table.Column<string>(type: "TEXT", nullable: true),
                    Yeast = table.Column<string>(type: "TEXT", nullable: true),
                    Malt = table.Column<string>(type: "TEXT", nullable: true),
                    BeerStyle = table.Column<string>(type: "TEXT", nullable: true),
                    BeerType = table.Column<string>(type: "TEXT", nullable: true),
                    CarbonationLevel = table.Column<string>(type: "TEXT", nullable: true),
                    Taste = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Code);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeChallengePart_ChallengeParts_PartialChallengesId",
                table: "ChallengeChallengePart",
                column: "PartialChallengesId",
                principalTable: "ChallengeParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeChallengePart_Challenges_ChallengesId",
                table: "ChallengeChallengePart",
                column: "ChallengesId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeUser_Challenges_UserId",
                table: "ChallengeUser",
                column: "UserId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeUser_Users_ChallengeId",
                table: "ChallengeUser",
                column: "ChallengeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeChallengePart_ChallengeParts_PartialChallengesId",
                table: "ChallengeChallengePart");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeChallengePart_Challenges_ChallengesId",
                table: "ChallengeChallengePart");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeUser_Challenges_UserId",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeUser_Users_ChallengeId",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrinkActions",
                table: "DrinkActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Challenges",
                table: "Challenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChallengeParts",
                table: "ChallengeParts");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "DrinkActions",
                newName: "DrinkAction");

            migrationBuilder.RenameTable(
                name: "Challenges",
                newName: "Challenge");

            migrationBuilder.RenameTable(
                name: "ChallengeParts",
                newName: "ChallengePart");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkActions_UserId",
                table: "DrinkAction",
                newName: "IX_DrinkAction_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrinkAction",
                table: "DrinkAction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Challenge",
                table: "Challenge",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChallengePart",
                table: "ChallengePart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeChallengePart_ChallengePart_PartialChallengesId",
                table: "ChallengeChallengePart",
                column: "PartialChallengesId",
                principalTable: "ChallengePart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeChallengePart_Challenge_ChallengesId",
                table: "ChallengeChallengePart",
                column: "ChallengesId",
                principalTable: "Challenge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeUser_Challenge_UserId",
                table: "ChallengeUser",
                column: "UserId",
                principalTable: "Challenge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeUser_User_ChallengeId",
                table: "ChallengeUser",
                column: "ChallengeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkAction_User_UserId",
                table: "DrinkAction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
