using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class donation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_GuildMembers_GuildId_MemberId",
                table: "GuildMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GuildMembers",
                nullable: false,
                defaultValueSql: "NEWID()");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                column: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GuildMembers",
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "GuildMemberDonations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Donation = table.Column<string>(maxLength: 100, nullable: true),
                    GuildMemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMemberDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildMemberDonations_GuildMembers_GuildMemberId",
                        column: x => x.GuildMemberId,
                        principalTable: "GuildMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_MemberId",
                table: "GuildMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildId_MemberId",
                table: "GuildMembers",
                columns: new[] { "GuildId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildMemberDonations_GuildMemberId",
                table: "GuildMemberDonations",
                column: "GuildMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildMemberDonations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_MemberId",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_GuildId_MemberId",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GuildMembers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GuildMembers_GuildId_MemberId",
                table: "GuildMembers",
                columns: new[] { "GuildId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                columns: new[] { "MemberId", "GuildId" });
        }
    }
}
