using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class ChangeResetCodeEmailsTableToGeneratedCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_reset_code_emails",
                table: "reset_code_emails");

            migrationBuilder.RenameTable(
                name: "reset_code_emails",
                newName: "generated_codes");

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "generated_codes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_generated_codes",
                table: "generated_codes",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_generated_codes",
                table: "generated_codes");

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "generated_codes");

            migrationBuilder.RenameTable(
                name: "generated_codes",
                newName: "reset_code_emails");

            migrationBuilder.AddPrimaryKey(
                name: "pk_reset_code_emails",
                table: "reset_code_emails",
                column: "id");
        }
    }
}
