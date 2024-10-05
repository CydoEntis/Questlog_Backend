using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRolesAndLinkedCharacterToPartyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetUsers_UserId",
                table: "PartyMembers");

            migrationBuilder.DropTable(
                name: "UserLevels");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PartyMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "PartyMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedOn",
                table: "PartyMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "PartyMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Parties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Parties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Adventures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Adventures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                name: "FK_PartyMembers_Characters_CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "JoinedOn",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Adventures");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Adventures");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "UserLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentExp = table.Column<int>(type: "int", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLevels_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_ApplicationUserId",
                table: "UserLevels",
                column: "ApplicationUserId");

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
