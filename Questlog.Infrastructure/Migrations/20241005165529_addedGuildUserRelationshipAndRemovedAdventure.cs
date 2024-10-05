using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedGuildUserRelationshipAndRemovedAdventure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Adventures_AdventureId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "Adventures");

            migrationBuilder.DropIndex(
                name: "IX_Parties_AdventureId",
                table: "Parties");

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

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
                name: "IX_Parties_AdventureId",
                table: "Parties",
                column: "AdventureId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Guilds_AdventureId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "ApplicationUserGuild");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Parties_AdventureId",
                table: "Parties");

            migrationBuilder.CreateTable(
                name: "Adventures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_AdventureId",
                table: "Parties",
                column: "AdventureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Adventures_AdventureId",
                table: "Parties",
                column: "AdventureId",
                principalTable: "Adventures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
