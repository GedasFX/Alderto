using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class makemodroleoptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildBanks",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildBanks",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
