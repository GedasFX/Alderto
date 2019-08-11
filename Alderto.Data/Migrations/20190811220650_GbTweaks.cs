using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class GbTweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildBankTransactions_GuildBankItems_ItemId",
                table: "GuildBankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GuildBanks_GuildId",
                table: "GuildBanks");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "GuildBankTransactions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "AdminId",
                table: "GuildBankTransactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "GuildBankTransactions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GuildBanks",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrencyCount",
                table: "GuildBanks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "GuildBankItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "GuildBankItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_GuildBanks_GuildId_Name",
                table: "GuildBanks",
                columns: new[] { "GuildId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildBankTransactions_GuildBankItems_ItemId",
                table: "GuildBankTransactions",
                column: "ItemId",
                principalTable: "GuildBankItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildBankTransactions_GuildBankItems_ItemId",
                table: "GuildBankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GuildBanks_GuildId_Name",
                table: "GuildBanks");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "GuildBankTransactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GuildBankTransactions");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "GuildBankItems");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "GuildBankItems");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "GuildBankTransactions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GuildBanks",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyCount",
                table: "GuildBanks",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.CreateIndex(
                name: "IX_GuildBanks_GuildId",
                table: "GuildBanks",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildBankTransactions_GuildBankItems_ItemId",
                table: "GuildBankTransactions",
                column: "ItemId",
                principalTable: "GuildBankItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
