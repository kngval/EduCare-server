using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class EmailInRTS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent");

            migrationBuilder.AlterColumn<int>(
                name: "UserInfoId",
                table: "RoomsToStudent",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RoomsToStudent",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RoomsToStudent");

            migrationBuilder.AlterColumn<int>(
                name: "UserInfoId",
                table: "RoomsToStudent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomsToStudent_UserInfo_UserInfoId",
                table: "RoomsToStudent",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id");
        }
    }
}
