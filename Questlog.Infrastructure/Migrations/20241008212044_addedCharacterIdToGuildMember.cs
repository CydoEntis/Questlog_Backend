using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCharacterIdToGuildMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PartyMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "GuildMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_CharacterId",
                table: "GuildMembers",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildMembers_Characters_CharacterId",
                table: "GuildMembers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildMembers_Characters_CharacterId",
                table: "GuildMembers");

            migrationBuilder.DropIndex(
                name: "IX_GuildMembers_CharacterId",
                table: "GuildMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "GuildMembers");
        }
    }
}
