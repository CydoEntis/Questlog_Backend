using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedGuildLeaderIdToGuildTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuildLeaderId",
                table: "Guilds",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_GuildLeaderId",
                table: "Guilds",
                column: "GuildLeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_AspNetUsers_GuildLeaderId",
                table: "Guilds",
                column: "GuildLeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_AspNetUsers_GuildLeaderId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_GuildLeaderId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "GuildLeaderId",
                table: "Guilds");
        }
    }
}
