using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class AddConversationMembership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User1ID",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "User1LastReadMessage",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "User2ID",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "User2LastReadMessage",
                table: "Conversations");

            migrationBuilder.CreateTable(
                name: "ConversationMemberships",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    ConversationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMemberships", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationMemberships");

            migrationBuilder.AddColumn<int>(
                name: "User1ID",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User1LastReadMessage",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2ID",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2LastReadMessage",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
