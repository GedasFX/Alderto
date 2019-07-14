using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class members : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildMembers_GuildMembers_RecruitedByMemberId",
                table: "GuildMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_GuildId",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_RecruitedByMemberId",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_MemberId_GuildId",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "RecruitedByMemberId",
                table: "GuildMembers");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PremiumUntil",
                table: "Guilds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CurrencyLastClaimed",
                table: "GuildMembers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "JoinedAt",
                table: "GuildMembers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "GuildMembers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RecruiterMemberId",
                table: "GuildMembers",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GuildMembers_GuildId_MemberId",
                table: "GuildMembers",
                columns: new[] { "GuildId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                columns: new[] { "MemberId", "GuildId" });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMembers_Members_MemberId",
                table: "GuildMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildMembers_Members_MemberId",
                table: "GuildMembers");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_GuildMembers_GuildId_MemberId",
                table: "GuildMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "RecruiterMemberId",
                table: "GuildMembers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PremiumUntil",
                table: "Guilds",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrencyLastClaimed",
                table: "GuildMembers",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GuildMembers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RecruitedByMemberId",
                table: "GuildMembers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildMembers",
                table: "GuildMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildId",
                table: "GuildMembers",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_RecruitedByMemberId",
                table: "GuildMembers",
                column: "RecruitedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_MemberId_GuildId",
                table: "GuildMembers",
                columns: new[] { "MemberId", "GuildId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMembers_GuildMembers_RecruitedByMemberId",
                table: "GuildMembers",
                column: "RecruitedByMemberId",
                principalTable: "GuildMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
