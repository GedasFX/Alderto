using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class AddTimelyDateToWallets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimelyLastClaimed",
                table: "GuildMemberWallets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimelyLastClaimed",
                table: "GuildMemberWallets");
        }
    }
}
