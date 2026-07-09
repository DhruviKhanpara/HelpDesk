using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserLoginHistoryAndRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                schema: "audit",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                schema: "security",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Token",
                schema: "security",
                table: "RefreshTokens",
                newName: "TokenHash");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_Token",
                schema: "security",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_TokenHash");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "audit",
                table: "LoginHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "FailureReason",
                schema: "audit",
                table: "LoginHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                schema: "audit",
                table: "LoginHistory",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                schema: "audit",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                schema: "audit",
                table: "LoginHistory");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                schema: "security",
                table: "RefreshTokens",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_TokenHash",
                schema: "security",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_Token");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                schema: "audit",
                table: "LoginHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                schema: "audit",
                table: "LoginHistory",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
