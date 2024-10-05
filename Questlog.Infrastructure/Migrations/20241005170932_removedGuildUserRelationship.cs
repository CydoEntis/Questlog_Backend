using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedGuildUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Guilds_AdventureId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetRoles_RoleId",
                table: "PartyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_Characters_CharacterId",
                table: "PartyMembers");

            migrationBuilder.DropTable(
                name: "ApplicationUserGuild");

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
                name: "RoleId",
                table: "PartyMembers");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "PartyMembers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "AdventureId",
                table: "Parties",
                newName: "GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Parties_AdventureId",
                table: "Parties",
                newName: "IX_Parties_GuildId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "PartyMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Guilds_GuildId",
                table: "Parties",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyMembers_AspNetUsers_UserId",
                table: "PartyMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Guilds_GuildId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyMembers_AspNetUsers_UserId",
                table: "PartyMembers");

            migrationBuilder.DropIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "PartyMembers");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PartyMembers",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "GuildId",
                table: "Parties",
                newName: "AdventureId");

            migrationBuilder.RenameIndex(
                name: "IX_Parties_GuildId",
                table: "Parties",
                newName: "IX_Parties_AdventureId");

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

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PartyMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserGuild",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    PartyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserGuild", x => new { x.ApplicationUserId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserGuild_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserGuild_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserGuild_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_CharacterId",
                table: "PartyMembers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_RoleId",
                table: "PartyMembers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGuild_GuildId",
                table: "ApplicationUserGuild",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGuild_PartyId",
                table: "ApplicationUserGuild",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Guilds_AdventureId",
                table: "Parties",
                column: "AdventureId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
