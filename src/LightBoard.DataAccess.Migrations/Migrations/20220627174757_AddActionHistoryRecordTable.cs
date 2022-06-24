using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddActionHistoryRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "action_history_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_type = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    action_type = table.Column<int>(type: "integer", nullable: false),
                    old_value = table.Column<string>(type: "text", nullable: true),
                    new_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_action_history_records", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "action_history_records");
        }
    }
}
