using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddColumnsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_board_members_boards_board_id",
                table: "board_members");

            migrationBuilder.CreateTable(
                name: "columns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    board_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_columns", x => x.id);
                    table.ForeignKey(
                        name: "fk_columns_boards_board_temp_id1",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_columns_board_id",
                table: "columns",
                column: "board_id");

            migrationBuilder.AddForeignKey(
                name: "fk_board_members_boards_board_temp_id",
                table: "board_members",
                column: "board_id",
                principalTable: "boards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_board_members_boards_board_temp_id",
                table: "board_members");

            migrationBuilder.DropTable(
                name: "columns");

            migrationBuilder.AddForeignKey(
                name: "fk_board_members_boards_board_id",
                table: "board_members",
                column: "board_id",
                principalTable: "boards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
