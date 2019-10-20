using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class MessagesContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "GuildManagedMessages",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                table: "GuildManagedMessages",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildManagedMessages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GuildBankItems",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(70)",
                oldMaxLength: 70,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LuaCode",
                table: "CustomCommands",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "GuildManagedMessages");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "GuildManagedMessages");

            migrationBuilder.DropColumn(
                name: "ModeratorRoleId",
                table: "GuildManagedMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GuildBankItems",
                type: "character varying(70)",
                maxLength: 70,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "LuaCode",
                table: "CustomCommands",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000);
        }
    }
}
