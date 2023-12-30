using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationship4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal",
                column: "ContentCommentId",
                principalTable: "ContentComments",
                principalColumn: "ContentCommentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTotal_ContentComments_ContentCommentId",
                table: "ContentTotal",
                column: "ContentCommentId",
                principalTable: "ContentComments",
                principalColumn: "ContentCommentId");
        }
    }
}
