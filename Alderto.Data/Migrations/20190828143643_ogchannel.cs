using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class ogchannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildBankTransactions");

            migrationBuilder.AddColumn<decimal>(
                name: "LogChannelId",
                table: "GuildBanks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogChannelId",
                table: "GuildBanks");

            migrationBuilder.CreateTable(
                name: "GuildBankTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<decimal>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(maxLength: 140, nullable: true),
                    ItemId = table.Column<int>(nullable: true),
                    MemberId = table.Column<decimal>(nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildBankTransactions_GuildBanks_BankId",
                        column: x => x.BankId,
                        principalTable: "GuildBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildBankTransactions_GuildBankItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "GuildBankItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildBankTransactions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankTransactions_BankId",
                table: "GuildBankTransactions",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankTransactions_ItemId",
                table: "GuildBankTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankTransactions_MemberId",
                table: "GuildBankTransactions",
                column: "MemberId");
        }
    }
}
