using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace website.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logging");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.EnsureSchema(
                name: "gamedata");

            migrationBuilder.EnsureSchema(
                name: "statistics");

            migrationBuilder.CreateTable(
                name: "BoxSets",
                schema: "gamedata",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    PublicationDate = table.Column<string>(maxLength: 25, nullable: true),
                    WikiLink = table.Column<string>(maxLength: 75, nullable: true),
                    Image = table.Column<string>(maxLength: 75, nullable: true)
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
                    Username = table.Column<string>(maxLength: 250, nullable: false),
                    Profile = table.Column<string>(maxLength: 500, nullable: true),
                    UserIcon = table.Column<string>(maxLength: 75, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 250, nullable: true),
                    HasClaimed = table.Column<bool>(nullable: false),
                    Locked = table.Column<bool>(nullable: false)
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
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    WikiLink = table.Column<string>(maxLength: 75, nullable: true),
                    Image = table.Column<string>(maxLength: 75, nullable: true),
                    BoxSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEnvironments", x => x.ID);
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
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Team = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    WikiLink = table.Column<string>(maxLength: 75, nullable: true),
                    PrintedComplexity = table.Column<int>(nullable: false),
                    IsAlt = table.Column<bool>(nullable: false),
                    BaseHero = table.Column<string>(maxLength: 250, nullable: true),
                    BoxSetId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(maxLength: 75, nullable: true)
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
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Type = table.Column<string>(nullable: false),
                    BaseName = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    WikiLink = table.Column<string>(maxLength: 75, nullable: true),
                    Image = table.Column<string>(maxLength: 75, nullable: true),
                    PrintedDifficulty = table.Column<int>(nullable: false),
                    BoxSetId = table.Column<int>(nullable: false)
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
                name: "AccessChanges",
                schema: "logging",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    AccessLevelId = table.Column<int>(nullable: false),
                    ChangedOn = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessChanges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccessChanges_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalSchema: "users",
                        principalTable: "AccessLevels",
                        principalColumn: "Level");
                    table.ForeignKey(
                        name: "FK_AccessChanges_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                schema: "logging",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
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
                    UserId = table.Column<int>(nullable: true),
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
                name: "GameDetails",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(nullable: false),
                    PerceivedDifficulty = table.Column<int>(nullable: false),
                    GameMode = table.Column<string>(nullable: false),
                    SelectionMethod = table.Column<string>(nullable: false),
                    Platform = table.Column<string>(nullable: false),
                    GameEndCondition = table.Column<string>(nullable: false),
                    GameTimeLength = table.Column<string>(nullable: false),
                    HouseRule = table.Column<string>(nullable: false),
                    NumberOfPlayers = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: true),
                    Win = table.Column<bool>(nullable: false),
                    UserComment = table.Column<string>(maxLength: 5000, nullable: true),
                    OtherSelections = table.Column<string>(maxLength: 1000, nullable: true),
                    CalculatedDifficulty = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameDetails_Games_GameId",
                        column: x => x.GameId,
                        principalSchema: "statistics",
                        principalTable: "Games",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentsUsed",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameDetailId = table.Column<int>(nullable: false),
                    GameEnvironmentId = table.Column<int>(nullable: false),
                    Destroyed = table.Column<bool>(nullable: false),
                    OblivAeonZone = table.Column<int>(nullable: false),
                    OblivAeonOrderAppeared = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentsUsed", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EnvironmentsUsed_GameDetails_GameDetailId",
                        column: x => x.GameDetailId,
                        principalSchema: "statistics",
                        principalTable: "GameDetails",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EnvironmentsUsed_GameEnvironments_GameEnvironmentId",
                        column: x => x.GameEnvironmentId,
                        principalSchema: "gamedata",
                        principalTable: "GameEnvironments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HeroTeams",
                schema: "statistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameDetailId = table.Column<int>(nullable: false),
                    HeroId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Incapped = table.Column<bool>(nullable: false),
                    OblivAeonIncapOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HeroTeams_GameDetails_GameDetailId",
                        column: x => x.GameDetailId,
                        principalSchema: "statistics",
                        principalTable: "GameDetails",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HeroTeams_HeroCharacters_HeroId",
                        column: x => x.HeroId,
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
                    Position = table.Column<int>(nullable: false),
                    Incapped = table.Column<bool>(nullable: false),
                    OblivAeon = table.Column<bool>(nullable: false),
                    VillainId = table.Column<int>(nullable: false),
                    GameDetailID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillainTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VillainTeams_GameDetails_GameDetailID",
                        column: x => x.GameDetailID,
                        principalSchema: "statistics",
                        principalTable: "GameDetails",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VillainTeams_VillainCharacters_VillainId",
                        column: x => x.VillainId,
                        principalSchema: "gamedata",
                        principalTable: "VillainCharacters",
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
                name: "IX_AccessChanges_AccessLevelId",
                schema: "logging",
                table: "AccessChanges",
                column: "AccessLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessChanges_UserId",
                schema: "logging",
                table: "AccessChanges",
                column: "UserId");

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
                name: "IX_EnvironmentsUsed_GameDetailId",
                schema: "statistics",
                table: "EnvironmentsUsed",
                column: "GameDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentsUsed_GameEnvironmentId",
                schema: "statistics",
                table: "EnvironmentsUsed",
                column: "GameEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDetails_GameId",
                schema: "statistics",
                table: "GameDetails",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                schema: "statistics",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_GameDetailId",
                schema: "statistics",
                table: "HeroTeams",
                column: "GameDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_HeroTeams_HeroId",
                schema: "statistics",
                table: "HeroTeams",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_GameDetailID",
                schema: "statistics",
                table: "VillainTeams",
                column: "GameDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_VillainTeams_VillainId",
                schema: "statistics",
                table: "VillainTeams",
                column: "VillainId");

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
                name: "AccessChanges",
                schema: "logging");

            migrationBuilder.DropTable(
                name: "LoginAttempts",
                schema: "logging");

            migrationBuilder.DropTable(
                name: "PasswordHistories",
                schema: "logging");

            migrationBuilder.DropTable(
                name: "EnvironmentsUsed",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "HeroTeams",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "VillainTeams",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "UserPermissions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "GameEnvironments",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "HeroCharacters",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "GameDetails",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "VillainCharacters",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "AccessLevels",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "statistics");

            migrationBuilder.DropTable(
                name: "BoxSets",
                schema: "gamedata");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "users");
        }
    }
}
