using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class guildconfigurationadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands");

            migrationBuilder.DropIndex(
                name: "IX_CustomCommands_GuildId_TriggerKeyword",
                table: "CustomCommands");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomCommands");

            migrationBuilder.AlterColumn<string>(
                name: "TriggerKeyword",
                table: "CustomCommands",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands",
                columns: new[] { "GuildId", "TriggerKeyword" });

            migrationBuilder.CreateTable(
                name: "GuildPreferences",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    Prefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildPreferences_Guilds_Id",
                        column: x => x.Id,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Guilds",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TriggerKeyword",
                table: "CustomCommands",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CustomCommands",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomCommands",
                table: "CustomCommands",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomCommands_GuildId_TriggerKeyword",
                table: "CustomCommands",
                columns: new[] { "GuildId", "TriggerKeyword" },
                unique: true,
                filter: "[TriggerKeyword] IS NOT NULL");
        }
    }
}
