using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class MessageModuleRecipientIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CheckLaterLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CheckLaterLinkCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MessageServiceRecipientId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckLaterLinks_UserId",
                table: "CheckLaterLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLaterLinkCategories_UserId",
                table: "CheckLaterLinkCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckLaterLinkCategories_AspNetUsers_UserId",
                table: "CheckLaterLinkCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckLaterLinks_AspNetUsers_UserId",
                table: "CheckLaterLinks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckLaterLinkCategories_AspNetUsers_UserId",
                table: "CheckLaterLinkCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckLaterLinks_AspNetUsers_UserId",
                table: "CheckLaterLinks");

            migrationBuilder.DropIndex(
                name: "IX_CheckLaterLinks_UserId",
                table: "CheckLaterLinks");

            migrationBuilder.DropIndex(
                name: "IX_CheckLaterLinkCategories_UserId",
                table: "CheckLaterLinkCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CheckLaterLinks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CheckLaterLinkCategories");

            migrationBuilder.DropColumn(
                name: "MessageServiceRecipientId",
                table: "AspNetUsers");
        }
    }
}
