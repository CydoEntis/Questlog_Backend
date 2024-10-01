using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedNameFromUserLevelToUserLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLevel_AspNetUsers_ApplicationUserId",
                table: "UserLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLevel",
                table: "UserLevel");

            migrationBuilder.RenameTable(
                name: "UserLevel",
                newName: "UserLevels");

            migrationBuilder.RenameIndex(
                name: "IX_UserLevel_ApplicationUserId",
                table: "UserLevels",
                newName: "IX_UserLevels_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLevels",
                table: "UserLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLevels_AspNetUsers_ApplicationUserId",
                table: "UserLevels",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLevels_AspNetUsers_ApplicationUserId",
                table: "UserLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLevels",
                table: "UserLevels");

            migrationBuilder.RenameTable(
                name: "UserLevels",
                newName: "UserLevel");

            migrationBuilder.RenameIndex(
                name: "IX_UserLevels_ApplicationUserId",
                table: "UserLevel",
                newName: "IX_UserLevel_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLevel",
                table: "UserLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLevel_AspNetUsers_ApplicationUserId",
                table: "UserLevel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
