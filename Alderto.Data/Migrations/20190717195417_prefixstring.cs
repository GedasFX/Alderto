using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class prefixstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildPreferences_Guilds_Id",
                table: "GuildPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildPreferences",
                table: "GuildPreferences");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GuildPreferences");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "GuildPreferences",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "GuildPreferences",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildPreferences",
                table: "GuildPreferences",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildPreferences_Guilds_GuildId",
                table: "GuildPreferences",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildPreferences_Guilds_GuildId",
                table: "GuildPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildPreferences",
                table: "GuildPreferences");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "GuildPreferences");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "GuildPreferences",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Id",
                table: "GuildPreferences",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildPreferences",
                table: "GuildPreferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildPreferences_Guilds_Id",
                table: "GuildPreferences",
                column: "Id",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
