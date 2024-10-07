using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedPartyMemberToHoldGuildMemberIdInsteadOfGuildId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_GuildMembers_GuildId",
                table: "PartyMembers");

            migrationBuilder.RenameColumn(
                name: "GuildId",
                table: "PartyMembers",
                newName: "GuildMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyMembers_GuildId",
                table: "PartyMembers",
                newName: "IX_PartyMembers_GuildMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_GuildMembers_GuildMemberId",
                table: "PartyMembers",
                column: "GuildMemberId",
                principalTable: "GuildMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_GuildMembers_GuildMemberId",
                table: "PartyMembers");

            migrationBuilder.RenameColumn(
                name: "GuildMemberId",
                table: "PartyMembers",
                newName: "GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyMembers_GuildMemberId",
                table: "PartyMembers",
                newName: "IX_PartyMembers_GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_GuildMembers_GuildId",
                table: "PartyMembers",
                column: "GuildId",
                principalTable: "GuildMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
