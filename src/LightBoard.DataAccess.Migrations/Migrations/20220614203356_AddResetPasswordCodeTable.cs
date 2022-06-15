using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddResetPasswordCodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reset_code_emails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reset_code = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reset_code_emails", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reset_code_emails");
        }
    }
}
