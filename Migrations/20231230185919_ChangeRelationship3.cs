using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationship3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal",
                column: "ContentPostId",
                principalTable: "ContentPosts",
                principalColumn: "ContentPostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal",
                column: "ContentPostId",
                principalTable: "ContentPosts",
                principalColumn: "ContentPostId");
        }
    }
}
