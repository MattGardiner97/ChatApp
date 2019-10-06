using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class ChangeFriendshipModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.AddColumn<byte[]>(
                name: "FriendIDBinary",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendIDBinary",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    User1ID = table.Column<int>(type: "int", nullable: false),
                    user2ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.User1ID, x.user2ID });
                    table.ForeignKey(
                        name: "FK_Friendship_AspNetUsers_User1ID",
                        column: x => x.User1ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendship_AspNetUsers_user2ID",
                        column: x => x.user2ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_user2ID",
                table: "Friendship",
                column: "user2ID");
        }
    }
}
