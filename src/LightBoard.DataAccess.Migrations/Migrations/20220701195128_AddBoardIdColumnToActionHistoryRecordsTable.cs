using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    public partial class AddBoardIdColumnToActionHistoryRecordsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "board_id",
                table: "action_history_records",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "ix_action_history_records_board_id",
                table: "action_history_records",
                column: "board_id");

            migrationBuilder.AddForeignKey(
                name: "fk_action_history_records_boards_board_temp_id2",
                table: "action_history_records",
                column: "board_id",
                principalTable: "boards",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_action_history_records_boards_board_temp_id2",
                table: "action_history_records");

            migrationBuilder.DropIndex(
                name: "ix_action_history_records_board_id",
                table: "action_history_records");

            migrationBuilder.DropColumn(
                name: "board_id",
                table: "action_history_records");
        }
    }
}
