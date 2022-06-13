using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddBoardMembersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "board_members",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    board_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_board_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_board_members_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_board_members_users_user_temp_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_board_members_board_id",
                table: "board_members",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "ix_board_members_user_id",
                table: "board_members",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "board_members");
        }
    }
}
