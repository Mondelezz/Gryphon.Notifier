using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLoginIdInUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserLoginId",
                table: "UserTokens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserLoginId",
                table: "UserTokens",
                column: "UserLoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_UserLogins_UserLoginId",
                table: "UserTokens",
                column: "UserLoginId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_UserLogins_UserLoginId",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserLoginId",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "UserLoginId",
                table: "UserTokens");
        }
    }
}
