using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedPartyMemberRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetRoles_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PartyMembers");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "PartyMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "PartyMembers");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: true);

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
        }
    }
}
