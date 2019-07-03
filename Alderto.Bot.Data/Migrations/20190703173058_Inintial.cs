using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Bot.Data.Migrations
{
    public partial class Inintial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<ulong>(nullable: false),
                    GuildId = table.Column<ulong>(nullable: false),
                    CurrencyLastClaimed = table.Column<DateTime>(nullable: true),
                    CurrencyCount = table.Column<int>(nullable: false),
                    RecruitedByMemberId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Members_RecruitedByMemberId",
                        column: x => x.RecruitedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_RecruitedByMemberId",
                table: "Members",
                column: "RecruitedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberId_GuildId",
                table: "Members",
                columns: new[] { "MemberId", "GuildId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
