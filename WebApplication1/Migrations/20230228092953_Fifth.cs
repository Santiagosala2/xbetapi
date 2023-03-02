using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthServer.Migrations
{
    /// <inheritdoc />
    public partial class Fifth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    BetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wager = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Climate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FriendClimate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    When = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FriendID = table.Column<int>(type: "int", nullable: false),
                    JudgeID = table.Column<int>(type: "int", nullable: false),
                    WinnerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.BetID);
                    table.ForeignKey(
                        name: "FK_Bets_Users_FriendID",
                        column: x => x.FriendID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Bets_Users_JudgeID",
                        column: x => x.JudgeID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Bets_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Bets_Users_WinnerID",
                        column: x => x.WinnerID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_FriendID",
                table: "Bets",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_JudgeID",
                table: "Bets",
                column: "JudgeID");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserID",
                table: "Bets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_WinnerID",
                table: "Bets",
                column: "WinnerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");
        }
    }
}
