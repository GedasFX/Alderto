using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class RemoveCurrencyFromGuildConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migrate over to new table
            migrationBuilder.Sql(
                "INSERT INTO \"Currencies\" (\"Id\", \"GuildId\", \"Name\",  \"Symbol\", \"TimelyAmount\", \"TimelyInterval\") " 
                    + "SELECT uuid_in(overlay(overlay(md5(random()::text || ':' || clock_timestamp()::text) placing '4' from 13) placing to_hex(floor(random()*(11-8+1) + 8)::int)::text from 17)::cstring), g.\"GuildId\", 'points', \"CurrencySymbol\", \"TimelyRewardQuantity\", \"TimelyCooldown\" " 
                    + "FROM (SELECT DISTINCT \"GuildId\" FROM \"GuildMembers\" WHERE \"CurrencyCount\" > 0) g "
                    + "INNER JOIN \"GuildPreferences\" gp ON g.\"GuildId\" = gp.\"GuildId\"");
            
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "GuildPreferences",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimelyCooldown",
                table: "GuildPreferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimelyRewardQuantity",
                table: "GuildPreferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
