using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class AddFriendship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendIDBinary",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<int>(nullable: false),
                    FriendID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Friendship_AspNetUsers_FriendID",
                        column: x => x.FriendID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendship_AspNetUsers_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_FriendID",
                table: "Friendship",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_OwnerID",
                table: "Friendship",
                column: "OwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.AddColumn<byte[]>(
                name: "FriendIDBinary",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
