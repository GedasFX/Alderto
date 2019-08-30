using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class moderationroleadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildBankContents");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "GuildBankItems");

            migrationBuilder.AddColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildBanks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ViewerRoleId",
                table: "GuildBanks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuildBankId",
                table: "GuildBankItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "GuildBankItems",
                maxLength: 140,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "GuildBankItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankItems_GuildBankId",
                table: "GuildBankItems",
                column: "GuildBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildBankItems_GuildBanks_GuildBankId",
                table: "GuildBankItems",
                column: "GuildBankId",
                principalTable: "GuildBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildBankItems_GuildBanks_GuildBankId",
                table: "GuildBankItems");

            migrationBuilder.DropIndex(
                name: "IX_GuildBankItems_GuildBankId",
                table: "GuildBankItems");

            migrationBuilder.DropColumn(
                name: "ModeratorRoleId",
                table: "GuildBanks");

            migrationBuilder.DropColumn(
                name: "ViewerRoleId",
                table: "GuildBanks");

            migrationBuilder.DropColumn(
                name: "GuildBankId",
                table: "GuildBankItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "GuildBankItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GuildBankItems");

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "GuildBankItems",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.CreateIndex(
                name: "IX_GuildBankContents_GuildBankItemId",
                table: "GuildBankContents",
                column: "GuildBankItemId");
        }
    }
}
