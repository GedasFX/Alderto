using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class RemoveCurrencyFromUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO \"GuildMemberWallets\" (\"Id\", \"CurrencyId\", \"MemberId\", \"Amount\", \"TimelyLastClaimed\") " +
                "SELECT uuid_in(overlay(overlay(md5(random()::text || ':' || clock_timestamp()::text) placing '4' from 13) placing to_hex(floor(random() * (11 - 8 + 1) + 8)::int)::text from 17)::cstring), " +
                    "c.\"Id\", \"MemberId\", \"CurrencyCount\", \"CurrencyLastClaimed\" " +
                "FROM \"GuildMembers\" o " +
                    "INNER JOIN \"Currencies\" c ON c.\"GuildId\" = o.\"GuildId\" " +
                "WHERE \"CurrencyCount\" > 0");
            
            migrationBuilder.DropColumn(
                name: "CurrencyCount",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "CurrencyLastClaimed",
                table: "GuildMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyCount",
                table: "GuildMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CurrencyLastClaimed",
                table: "GuildMembers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
