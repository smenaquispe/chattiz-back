using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chattiz_back.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessager",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "NumberOfMessages",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Chats");

            migrationBuilder.AddColumn<string>(
                name: "LastMessager",
                table: "ChatUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMessages",
                table: "ChatUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ChatUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessager",
                table: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfMessages",
                table: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChatUsers");

            migrationBuilder.AddColumn<string>(
                name: "LastMessager",
                table: "Chats",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMessages",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Chats",
                type: "int",
                nullable: true);
        }
    }
}
