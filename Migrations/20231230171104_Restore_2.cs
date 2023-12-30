using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class Restore_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotals_ContentComments_ContentCommentId",
                table: "ContentTotals");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotals_ContentPosts_ContentPostId",
                table: "ContentTotals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTotals",
                table: "ContentTotals");

            migrationBuilder.RenameTable(
                name: "ContentTotals",
                newName: "ContentTotal");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTotals_ContentPostId",
                table: "ContentTotal",
                newName: "IX_ContentTotal_ContentPostId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTotals_ContentCommentId",
                table: "ContentTotal",
                newName: "IX_ContentTotal_ContentCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTotal",
                table: "ContentTotal",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal",
                column: "ContentCommentId",
                principalTable: "ContentComments",
                principalColumn: "ContentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal",
                column: "ContentPostId",
                principalTable: "ContentPosts",
                principalColumn: "ContentPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentPosts_ContentPostId",
                table: "ContentTotal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTotal",
                table: "ContentTotal");

            migrationBuilder.RenameTable(
                name: "ContentTotal",
                newName: "ContentTotals");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTotal_ContentPostId",
                table: "ContentTotals",
                newName: "IX_ContentTotals_ContentPostId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTotal_ContentCommentId",
                table: "ContentTotals",
                newName: "IX_ContentTotals_ContentCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTotals",
                table: "ContentTotals",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotals_ContentComments_ContentCommentId",
                table: "ContentTotals",
                column: "ContentCommentId",
                principalTable: "ContentComments",
                principalColumn: "ContentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotals_ContentPosts_ContentPostId",
                table: "ContentTotals",
                column: "ContentPostId",
                principalTable: "ContentPosts",
                principalColumn: "ContentPostId");
        }
    }
}
