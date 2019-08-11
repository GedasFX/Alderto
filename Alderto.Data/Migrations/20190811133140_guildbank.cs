using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class guildbank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildMemberDonations");

            migrationBuilder.CreateTable(
                name: "GuildBankItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 70, nullable: true),
                    Description = table.Column<string>(maxLength: 280, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBankItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildBanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    CurrencyCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildBanks_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildBankContents",
                columns: table => new
                {
                    GuildBankId = table.Column<int>(nullable: false),
                    GuildBankItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBankContents", x => new { x.GuildBankId, x.GuildBankItemId });
                    table.ForeignKey(
                        name: "FK_GuildBankContents_GuildBanks_GuildBankId",
                        column: x => x.GuildBankId,
                        principalTable: "GuildBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildBankContents_GuildBankItems_GuildBankItemId",
                        column: x => x.GuildBankItemId,
                        principalTable: "GuildBankItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildBankTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTimeOffset>(nullable: false),
                    Comment = table.Column<string>(maxLength: 140, nullable: true),
                    ItemId = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    MemberId = table.Column<decimal>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildBankTransactions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankContents_GuildBankItemId",
                table: "GuildBankContents",
                column: "GuildBankItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBanks_GuildId",
                table: "GuildBanks",
                column: "GuildId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildBankContents");

            migrationBuilder.DropTable(
                name: "GuildBankTransactions");

            migrationBuilder.DropTable(
                name: "GuildBanks");

            migrationBuilder.DropTable(
                name: "GuildBankItems");

            migrationBuilder.CreateTable(
                name: "GuildMemberDonations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Donation = table.Column<string>(maxLength: 100, nullable: true),
                    DonationDate = table.Column<DateTimeOffset>(nullable: false),
                    GuildId = table.Column<decimal>(nullable: false),
                    MemberId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMemberDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildMemberDonations_GuildMembers_GuildId_MemberId",
                        columns: x => new { x.GuildId, x.MemberId },
                        principalTable: "GuildMembers",
                        principalColumns: new[] { "GuildId", "MemberId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildMemberDonations_GuildId_MemberId",
                table: "GuildMemberDonations",
                columns: new[] { "GuildId", "MemberId" });
        }
    }
}
