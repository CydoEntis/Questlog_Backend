using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedRelationshipBetweenApplicationUserAndCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Characters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ApplicationUserId",
                table: "Characters",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AspNetUsers_ApplicationUserId",
                table: "Characters",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AspNetUsers_ApplicationUserId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_ApplicationUserId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                unique: true,
                filter: "[CharacterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
