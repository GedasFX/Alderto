using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class RemoveCurrencyFromGuildConfig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildMemberWallets_GuildMembers_GuildId_MemberId",
                table: "GuildMemberWallets");

            migrationBuilder.DropIndex(
                name: "IX_GuildMemberWallets_GuildId_MemberId",
                table: "GuildMemberWallets");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "GuildMemberWallets");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMemberWallets_MemberId",
                table: "GuildMemberWallets",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMemberWallets_Members_MemberId",
                table: "GuildMemberWallets",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildMemberWallets_Members_MemberId",
                table: "GuildMemberWallets");

            migrationBuilder.DropIndex(
                name: "IX_GuildMemberWallets_MemberId",
                table: "GuildMemberWallets");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "GuildMemberWallets",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_GuildMemberWallets_GuildId_MemberId",
                table: "GuildMemberWallets",
                columns: new[] { "GuildId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMemberWallets_GuildMembers_GuildId_MemberId",
                table: "GuildMemberWallets",
                columns: new[] { "GuildId", "MemberId" },
                principalTable: "GuildMembers",
                principalColumns: new[] { "GuildId", "MemberId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
