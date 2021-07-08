using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class FixNoIqUnlinkedCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyTransactions_GuildMembers_GuildId_RecipientId",
                table: "CurrencyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyTransactions_GuildMembers_GuildId_SenderId",
                table: "CurrencyTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTransactions_GuildId_RecipientId",
                table: "CurrencyTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTransactions_GuildId_SenderId",
                table: "CurrencyTransactions");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "CurrencyTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "CurrencyTransactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_CurrencyId",
                table: "CurrencyTransactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_RecipientId",
                table: "CurrencyTransactions",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_SenderId",
                table: "CurrencyTransactions",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyTransactions_Currencies_CurrencyId",
                table: "CurrencyTransactions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyTransactions_Members_RecipientId",
                table: "CurrencyTransactions",
                column: "RecipientId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyTransactions_Members_SenderId",
                table: "CurrencyTransactions",
                column: "SenderId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyTransactions_Currencies_CurrencyId",
                table: "CurrencyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyTransactions_Members_RecipientId",
                table: "CurrencyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyTransactions_Members_SenderId",
                table: "CurrencyTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTransactions_CurrencyId",
                table: "CurrencyTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTransactions_RecipientId",
                table: "CurrencyTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyTransactions_SenderId",
                table: "CurrencyTransactions");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "CurrencyTransactions");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "CurrencyTransactions",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_GuildId_RecipientId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "RecipientId" });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransactions_GuildId_SenderId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "SenderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyTransactions_GuildMembers_GuildId_RecipientId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "RecipientId" },
                principalTable: "GuildMembers",
                principalColumns: new[] { "GuildId", "MemberId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyTransactions_GuildMembers_GuildId_SenderId",
                table: "CurrencyTransactions",
                columns: new[] { "GuildId", "SenderId" },
                principalTable: "GuildMembers",
                principalColumns: new[] { "GuildId", "MemberId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
