using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddSurnameColumnToUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "surname",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "Stub");

            migrationBuilder.Sql("ALTER TABLE users ALTER COLUMN surname DROP DEFAULT;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "surname",
                table: "users");
        }
    }
}
