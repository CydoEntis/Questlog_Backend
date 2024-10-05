using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedPartyMemberToAssociateCharacterInsteadOfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetUsers_UserId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PartyMembers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "PartyMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Parties",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Guilds",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Guilds",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_ApplicationUserId",
                table: "PartyMembers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_CharacterId",
                table: "PartyMembers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_RoleId",
                table: "PartyMembers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_AspNetRoles_RoleId",
                table: "PartyMembers",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_AspNetUsers_ApplicationUserId",
                table: "PartyMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_Characters_CharacterId",
                table: "PartyMembers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetRoles_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetUsers_ApplicationUserId",
                table: "PartyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_Characters_CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_ApplicationUserId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Guilds");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "PartyMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Guilds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_AspNetUsers_UserId",
                table: "PartyMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
