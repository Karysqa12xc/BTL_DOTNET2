using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseToMulipleFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentTotal",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentPostId = table.Column<int>(type: "int", nullable: true),
                    ContentCommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTotal", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_ContentTotal_ContentComments_ContentCommentId",
                        column: x => x.ContentCommentId,
                        principalTable: "ContentComments",
                        principalColumn: "ContentCommentId");
                    table.ForeignKey(
                        name: "FK_ContentTotal_ContentPosts_ContentPostId",
                        column: x => x.ContentPostId,
                        principalTable: "ContentPosts",
                        principalColumn: "ContentPostId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTotal_ContentCommentId",
                table: "ContentTotal",
                column: "ContentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTotal_ContentPostId",
                table: "ContentTotal",
                column: "ContentPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTotal");
        }
    }
}
