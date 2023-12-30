using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Img_Video : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "ContentPosts");

            migrationBuilder.DropColumn(
                name: "Videos",
                table: "ContentPosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "ContentPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Videos",
                table: "ContentPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
