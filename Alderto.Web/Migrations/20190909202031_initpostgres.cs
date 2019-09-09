using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Alderto.Web.Migrations
{
    public partial class initpostgres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    PremiumUntil = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    Username = table.Column<string>(maxLength: 32, nullable: true),
                    Discriminator = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomCommands",
                columns: table => new
                {
                    TriggerKeyword = table.Column<string>(maxLength: 20, nullable: false),
                    GuildId = table.Column<decimal>(nullable: false),
                    LuaCode = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCommands", x => new { x.GuildId, x.TriggerKeyword });
                    table.ForeignKey(
                        name: "FK_CustomCommands_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildBanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    GuildId = table.Column<decimal>(nullable: false),
                    LogChannelId = table.Column<decimal>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    ModeratorRoleId = table.Column<decimal>(nullable: false)
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
                name: "GuildPreferences",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 20, nullable: false),
                    CurrencySymbol = table.Column<string>(maxLength: 50, nullable: false),
                    TimelyRewardQuantity = table.Column<int>(nullable: false),
                    TimelyCooldown = table.Column<int>(nullable: false),
                    AcceptedMemberRoleId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildPreferences", x => x.GuildId);
                    table.ForeignKey(
                        name: "FK_GuildPreferences_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildMembers",
                columns: table => new
                {
                    MemberId = table.Column<decimal>(nullable: false),
                    GuildId = table.Column<decimal>(nullable: false),
                    Nickname = table.Column<string>(maxLength: 32, nullable: true),
                    CurrencyLastClaimed = table.Column<DateTimeOffset>(nullable: false),
                    JoinedAt = table.Column<DateTimeOffset>(nullable: true),
                    CurrencyCount = table.Column<int>(nullable: false),
                    RecruiterMemberId = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMembers", x => new { x.GuildId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_GuildMembers_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildBankItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    GuildBankId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 70, nullable: true),
                    Description = table.Column<string>(maxLength: 280, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 140, nullable: true),
                    Value = table.Column<double>(nullable: false),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBankItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildBankItems_GuildBanks_GuildBankId",
                        column: x => x.GuildBankId,
                        principalTable: "GuildBanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankItems_GuildBankId",
                table: "GuildBankItems",
                column: "GuildBankId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBanks_GuildId_Name",
                table: "GuildBanks",
                columns: new[] { "GuildId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_MemberId",
                table: "GuildMembers",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomCommands");

            migrationBuilder.DropTable(
                name: "GuildBankItems");

            migrationBuilder.DropTable(
                name: "GuildMembers");

            migrationBuilder.DropTable(
                name: "GuildPreferences");

            migrationBuilder.DropTable(
                name: "GuildBanks");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
