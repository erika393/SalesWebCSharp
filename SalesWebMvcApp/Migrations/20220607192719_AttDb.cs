using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesWebMvcApp.Migrations
{
    public partial class AttDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Seller");

            migrationBuilder.AddColumn<string>(
                name: "EmailUserAtt",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailUserAtt",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Seller",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
