using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class currencymodulepref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "GuildPreferences",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimelyCooldown",
                table: "GuildPreferences",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimelyRewardQuantity",
                table: "GuildPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                table: "GuildPreferences");

            migrationBuilder.DropColumn(
                name: "TimelyCooldown",
                table: "GuildPreferences");

            migrationBuilder.DropColumn(
                name: "TimelyRewardQuantity",
                table: "GuildPreferences");
        }
    }
}
