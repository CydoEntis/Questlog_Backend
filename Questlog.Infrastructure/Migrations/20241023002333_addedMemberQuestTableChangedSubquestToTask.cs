using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedMemberQuestTableChangedSubquestToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberQuest_Members_AssignedMemberId",
                table: "MemberQuest");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Quests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_MemberId",
                table: "Quests",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberQuest_Members_AssignedMemberId",
                table: "MemberQuest",
                column: "AssignedMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Members_MemberId",
                table: "Quests",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberQuest_Members_AssignedMemberId",
                table: "MemberQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Members_MemberId",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_MemberId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Quests");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberQuest_Members_AssignedMemberId",
                table: "MemberQuest",
                column: "AssignedMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
