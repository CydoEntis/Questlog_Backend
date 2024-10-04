using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCharacterAndUnlockablesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Archetype = table.Column<int>(type: "int", nullable: false),
                    CurrentExp = table.Column<int>(type: "int", nullable: false),
                    ExpToNextLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unlockables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Archetype = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unlockables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unlockables_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unlockables_CharacterId",
                table: "Unlockables",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Characters_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Unlockables");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CharacterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "AspNetUsers");
        }
    }
}
