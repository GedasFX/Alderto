using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class pkchangeyetagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("GuildMemberDonations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_GuildId_MemberId",
                table: "GuildMembers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GuildMembers_Id",
                table: "GuildMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                columns: new[] { "GuildId", "MemberId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildMemberDonations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_GuildMembers_Id",
                table: "GuildMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildId_MemberId",
                table: "GuildMembers",
                columns: new[] { "GuildId", "MemberId" },
                unique: true);
        }
    }
}
