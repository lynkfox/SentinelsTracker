using Microsoft.EntityFrameworkCore.Migrations;

namespace website.Data.Migrations
{
    public partial class AdjustingHeroAndVillainTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlt",
                schema: "gamedata",
                table: "VillainCharacters");

            migrationBuilder.AddColumn<string>(
                name: "Team",
                schema: "gamedata",
                table: "HeroCharacters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                schema: "gamedata",
                table: "HeroCharacters");

            migrationBuilder.AddColumn<bool>(
                name: "IsAlt",
                schema: "gamedata",
                table: "VillainCharacters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
