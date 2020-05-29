using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace website.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.EnsureSchema(
                name: "gamedata");

            migrationBuilder.EnsureSchema(
                name: "statistics");

            migrationBuilder.EnsureSchema(
                name: "logging");

            migrationBuilder.CreateTable(
                name: "BoxSets",
                schema: "gamedata",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PublicationDate = table.Column<string>(nullable: true),
                    WikiLink = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxSets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AccessLevels",
                schema: "users",
                columns: table => new
                {
                    Level = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Read = table.Column<bool>(nullable: false),
                    Modify = table.Column<bool>(nullable: false),
                    LevelCommonName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevels", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 250, nullable: true),
                    Profile = table.Column<string>(nullable: true),
                    UserIcon = table.Column<string>(nullable: true),
                    UserEmail = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GameEnvironments",
                schema: "gamedata",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    WikiLink = table.Column<string>(nullable: true),
                    BoxSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEnvironments", x => x.Name);
                    table.ForeignKey(
                        name: "FK_GameEnvironments_BoxSets_BoxSetId",
                        column: x => x.BoxSetId,
                        principalSchema: "gamedata",
                        principalTable: "BoxSets",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HeroCharacters",
                schema: "gamedata",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WikiLink = table.Column<string>(nullable: true),
                    PrintedComplexity = table.Column<int>(nullable: false),
                    IsAlt = table.Column<bool>(nullable: false),
                    BoxSetId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroCharacters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HeroCharacters_BoxSets_BoxSetId",
                        column: x => x.BoxSetId,
                        principalSchema: "gamedata",
                        principalTable: "BoxSets",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "VillainCharacters",
                schema: "gamedata",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    WikiLink = table.Column<string>(nullable: true),
                    PrintedDifficulty = table.Column<int>(nullable: false),
                    IsAlt = table.Column<bool>(nullable: false),
                    BoxSetId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillainCharacters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VillainCharacters_BoxSets_BoxSetId",
                        column: x => x.BoxSetId,
                        principalSchema: "gamedata",
                        principalTable: "BoxSets",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                schema: "logging",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: true),
                    AttemptTime = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LoginAttempts_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistories",
                schema: "logging",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PreviousSalt = table.Column<string>(nullable: true),
                    PreviousHash = table.Column<string>(nullable: true),
                    ChangedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PasswordHistories_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Platform = table.Column<string>(nullable: true),
                    DateEntered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                schema: "users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    AccessLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserPermissions_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalSchema: "users",
                        principalTable: "AccessLevels",
                        principalColumn: "Level");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HeroTeams",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstHeroId = table.Column<int>(nullable: true),
                    SecondHeroId = table.Column<int>(nullable: true),
                    ThirdHeroId = table.Column<int>(nullable: true),
                    FourthHeroId = table.Column<int>(nullable: true),
                    FifthHeroId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_FifthHeroId",
                        column: x => x.FifthHeroId,
                        principalSchema: "gamedata",
                        principalTable: "HeroCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_FirstHeroId",
                        column: x => x.FirstHeroId,
                        principalSchema: "gamedata",
                        principalTable: "HeroCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_FourthHeroId",
                        column: x => x.FourthHeroId,
                        principalSchema: "gamedata",
                        principalTable: "HeroCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_SecondHeroId",
                        column: x => x.SecondHeroId,
                        principalSchema: "gamedata",
                        principalTable: "HeroCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_ThirdHeroId",
                        column: x => x.ThirdHeroId,
                        principalSchema: "gamedata",
                        principalTable: "HeroCharacters",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "VillainTeams",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VillainTeamGame = table.Column<bool>(nullable: false),
                    OblivAeon = table.Column<bool>(nullable: false),
                    FirstVillainId = table.Column<int>(nullable: true),
                    SecondVillainId = table.Column<int>(nullable: true),
                    ThirdVillainId = table.Column<int>(nullable: true),
                    FourthVillainId = table.Column<int>(nullable: true),
                    FifthVillainId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillainTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_FifthVillainId",
                        column: x => x.FifthVillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_FirstVillainId",
                        column: x => x.FirstVillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_FourthVillainId",
                        column: x => x.FourthVillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_SecondVillainId",
                        column: x => x.SecondVillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_ThirdVillainId",
                        column: x => x.ThirdVillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "GameDetails",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(nullable: false),
                    VillainTeamId = table.Column<int>(nullable: false),
                    HeroTeamId = table.Column<int>(nullable: false),
                    EnvironmentName = table.Column<string>(nullable: false),
                    HeroPositionsIncap = table.Column<string>(maxLength: 10, nullable: true),
                    VillainPostionsIncap = table.Column<string>(maxLength: 10, nullable: true),
                    PerceivedDifficulty = table.Column<int>(nullable: false),
                    GameMode = table.Column<string>(nullable: false),
                    SelectionMethod = table.Column<string>(nullable: false),
                    Platform = table.Column<string>(nullable: false),
                    GameEndCondition = table.Column<string>(nullable: false),
                    GameTimeLength = table.Column<string>(nullable: true),
                    NumberOfPlayers = table.Column<int>(nullable: false),
                    HouseRules = table.Column<bool>(nullable: false),
                    Rounds = table.Column<int>(nullable: true),
                    Win = table.Column<bool>(nullable: false),
                    UserComment = table.Column<string>(maxLength: 1000, nullable: true),
                    CalculatedDifficulty = table.Column<double>(nullable: false),
                    GameEnvironmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameDetails_GameEnvironments_EnvironmentName",
                        column: x => x.EnvironmentName,
                        principalSchema: "gamedata",
                        principalTable: "GameEnvironments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_GameDetails_GameEnvironments_GameEnvironmentName",
                        column: x => x.GameEnvironmentName,
                        principalSchema: "gamedata",
                        principalTable: "GameEnvironments",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameDetails_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "statistics",
                        principalTable: "Games",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GameDetails_HeroTeams_HeroTeamId",
                        column: x => x.HeroTeamId,
                        principalSchema: "statistics",
                        principalTable: "HeroTeams",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GameDetails_VillainTeams_VillainTeamId",
                        column: x => x.VillainTeamId,
                        principalSchema: "statistics",
                        principalTable: "VillainTeams",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEnvironments_BoxSetId",
                schema: "gamedata",
                table: "GameEnvironments",
                column: "BoxSetId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroCharacters_BoxSetId",
                schema: "gamedata",
                table: "HeroCharacters",
                column: "BoxSetId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainCharacters_BoxSetId",
                schema: "gamedata",
                table: "VillainCharacters",
                column: "BoxSetId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_UserID",
                schema: "logging",
                table: "LoginAttempts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistories_UserId",
                schema: "logging",
                table: "PasswordHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_EnvironmentName",
                schema: "statistics",
                table: "GameDetails",
                column: "EnvironmentName");

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_GameEnvironmentName",
                schema: "statistics",
                table: "GameDetails",
                column: "GameEnvironmentName");

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_GameId",
                schema: "statistics",
                table: "GameDetails",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_HeroTeamId",
                schema: "statistics",
                table: "GameDetails",
                column: "HeroTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_VillainTeamId",
                schema: "statistics",
                table: "GameDetails",
                column: "VillainTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                schema: "statistics",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_FifthHeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "FifthHeroId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_FirstHeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "FirstHeroId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_FourthHeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "FourthHeroId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_SecondHeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "SecondHeroId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_ThirdHeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "ThirdHeroId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_FifthVillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "FifthVillainId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_FirstVillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "FirstVillainId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_FourthVillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "FourthVillainId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_SecondVillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "SecondVillainId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_ThirdVillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "ThirdVillainId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_AccessLevelId",
                schema: "users",
                table: "UserPermissions",
                column: "AccessLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                schema: "users",
                table: "UserPermissions",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttempts",
                schema: "logging");

            migrationBuilder.DropTable(
                name: "PasswordHistories",
                schema: "logging");

            migrationBuilder.DropTable(
                name: "GameDetails",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "UserPermissions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "GameEnvironments",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "HeroTeams",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "VillainTeams",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "AccessLevels",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "users");

            migrationBuilder.DropTable(
                name: "HeroCharacters",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "VillainCharacters",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "BoxSets",
                schema: "gamedata");
        }
    }
}
