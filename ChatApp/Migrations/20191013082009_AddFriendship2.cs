using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class AddFriendship2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendID",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_OwnerID",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendships");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_OwnerID",
                table: "Friendships",
                newName: "IX_Friendships_OwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_FriendID",
                table: "Friendships",
                newName: "IX_Friendships_FriendID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendID",
                table: "Friendships",
                column: "FriendID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_OwnerID",
                table: "Friendships",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendID",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_OwnerID",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "Friendship");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_OwnerID",
                table: "Friendship",
                newName: "IX_Friendship_OwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FriendID",
                table: "Friendship",
                newName: "IX_Friendship_FriendID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendID",
                table: "Friendship",
                column: "FriendID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_OwnerID",
                table: "Friendship",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
