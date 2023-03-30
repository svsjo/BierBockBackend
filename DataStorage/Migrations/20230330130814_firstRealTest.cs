using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorage.Migrations
{
    /// <inheritdoc />
    public partial class firstRealTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions");

            migrationBuilder.DropTable(
                name: "ChallengeChallengePart");

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

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "DrinkActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Challenges",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BeerCode",
                table: "ChallengeParts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChallengePartChallenge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChallengeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChallengePartId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengePartChallenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengePartChallenge_ChallengeParts_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "ChallengeParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengePartChallenge_Challenges_ChallengePartId",
                        column: x => x.ChallengePartId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengePartChallenge_ChallengeId",
                table: "ChallengePartChallenge",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengePartChallenge_ChallengePartId",
                table: "ChallengePartChallenge",
                column: "ChallengePartId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions");

            migrationBuilder.DropTable(
                name: "ChallengePartChallenge");

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
                name: "Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "BeerCode",
                table: "ChallengeParts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "DrinkActions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "ChallengeChallengePart",
                columns: table => new
                {
                    ChallengesId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartialChallengesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeChallengePart", x => new { x.ChallengesId, x.PartialChallengesId });
                    table.ForeignKey(
                        name: "FK_ChallengeChallengePart_ChallengeParts_PartialChallengesId",
                        column: x => x.PartialChallengesId,
                        principalTable: "ChallengeParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeChallengePart_Challenges_ChallengesId",
                        column: x => x.ChallengesId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeChallengePart_PartialChallengesId",
                table: "ChallengeChallengePart",
                column: "PartialChallengesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkActions_Users_UserId",
                table: "DrinkActions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
