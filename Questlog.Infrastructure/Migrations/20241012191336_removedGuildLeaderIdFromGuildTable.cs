using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedGuildLeaderIdFromGuildTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_GuildMembers_GuildLeaderId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_GuildLeaderId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "GuildLeaderId",
                table: "Guilds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuildLeaderId",
                table: "Guilds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_GuildLeaderId",
                table: "Guilds",
                column: "GuildLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_GuildMembers_GuildLeaderId",
                table: "Guilds",
                column: "GuildLeaderId",
                principalTable: "GuildMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
