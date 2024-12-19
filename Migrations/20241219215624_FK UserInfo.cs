using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class FKUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "RoomsToStudent",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomsToStudent_RoomId",
                table: "RoomsToStudent",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsToStudent_UserInfoId",
                table: "RoomsToStudent",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsToStudent_Rooms_RoomId",
                table: "RoomsToStudent",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomsToStudent_Rooms_RoomId",
                table: "RoomsToStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent");

            migrationBuilder.DropIndex(
                name: "IX_RoomsToStudent_RoomId",
                table: "RoomsToStudent");

            migrationBuilder.DropIndex(
                name: "IX_RoomsToStudent_UserInfoId",
                table: "RoomsToStudent");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "RoomsToStudent");
        }
    }
}
