using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "CheckLaterLinkCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckLaterLinkCategories_ParentCategoryId",
                table: "CheckLaterLinkCategories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckLaterLinkCategories_CheckLaterLinkCategories_ParentCat~",
                table: "CheckLaterLinkCategories",
                column: "ParentCategoryId",
                principalTable: "CheckLaterLinkCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckLaterLinkCategories_CheckLaterLinkCategories_ParentCat~",
                table: "CheckLaterLinkCategories");

            migrationBuilder.DropIndex(
                name: "IX_CheckLaterLinkCategories_ParentCategoryId",
                table: "CheckLaterLinkCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "CheckLaterLinkCategories");
        }
    }
}
