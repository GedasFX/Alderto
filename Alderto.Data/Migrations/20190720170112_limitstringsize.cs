using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class limitstringsize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Members",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Members",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "GuildPreferences",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySymbol",
                table: "GuildPreferences",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "GuildMembers",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LuaCode",
                table: "CustomCommands",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands");


            migrationBuilder.AlterColumn<string>(
                name: "TriggerKeyword",
                table: "CustomCommands",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands",
                columns: new[] { "GuildId", "TriggerKeyword" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Members",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Members",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "GuildPreferences",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySymbol",
                table: "GuildPreferences",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "GuildMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LuaCode",
                table: "CustomCommands",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands");

            migrationBuilder.AlterColumn<string>(
                name: "TriggerKeyword",
                table: "CustomCommands",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands",
                columns: new[] { "GuildId", "TriggerKeyword" });
        }
    }
}
