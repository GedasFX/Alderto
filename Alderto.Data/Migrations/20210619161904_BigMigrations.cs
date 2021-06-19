using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Alderto.Data.Migrations
{
    public partial class BigMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Currencies_GuildId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "AcceptedMemberRoleId",
                table: "GuildPreferences");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "GuildMemberWallets",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "CurrencySymbol",
                table: "Currencies",
                newName: "Symbol");

            migrationBuilder.AddColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildPreferences",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimelyAmount",
                table: "Currencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SenderId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RecipientId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    IsAward = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyTransactions_GuildMembers_GuildId_RecipientId",
                        columns: x => new { x.GuildId, x.RecipientId },
                        principalTable: "GuildMembers",
                        principalColumns: new[] { "GuildId", "MemberId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyTransactions_GuildMembers_GuildId_SenderId",
                        columns: x => new { x.GuildId, x.SenderId },
                        principalTable: "GuildMembers",
                        principalColumns: new[] { "GuildId", "MemberId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildCommandAliases",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Command = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildCommandAliases", x => new { x.GuildId, x.Alias });
                    table.ForeignKey(
                        name: "FK_GuildCommandAliases_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_GuildId_Name",
                table: "Currencies",
                columns: new[] { "GuildId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_GuildId_RecipientId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "RecipientId" });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_GuildId_SenderId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "SenderId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyTransactions");

            migrationBuilder.DropTable(
                name: "GuildCommandAliases");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_GuildId_Name",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ModeratorRoleId",
                table: "GuildPreferences");

            migrationBuilder.DropColumn(
                name: "TimelyAmount",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GuildMemberWallets",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "Symbol",
                table: "Currencies",
                newName: "CurrencySymbol");

            migrationBuilder.AddColumn<decimal>(
                name: "AcceptedMemberRoleId",
                table: "GuildPreferences",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_GuildId",
                table: "Currencies",
                column: "GuildId");
        }
    }
}
