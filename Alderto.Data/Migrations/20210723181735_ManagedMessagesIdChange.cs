using Microsoft.EntityFrameworkCore.Migrations;

namespace Alderto.Data.Migrations
{
    public partial class ManagedMessagesIdChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModeratorRoleId",
                table: "GuildManagedMessages");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "GuildManagedMessages",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GuildManagedMessages",
                newName: "MessageId");

            migrationBuilder.AddColumn<decimal>(
                name: "ModeratorRoleId",
                table: "GuildManagedMessages",
                type: "numeric(20,0)",
                nullable: true);
        }
    }
}
