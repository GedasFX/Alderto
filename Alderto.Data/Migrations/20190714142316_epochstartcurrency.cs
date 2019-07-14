using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class epochstartcurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CurrencyLastClaimed",
                table: "GuildMembers",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CurrencyLastClaimed",
                table: "GuildMembers",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));
        }
    }
}
