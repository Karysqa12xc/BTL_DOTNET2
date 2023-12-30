using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class Add_Image_List : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "ContentPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "ContentPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Videos",
                table: "ContentPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "ContentComments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "ContentPosts");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "ContentPosts");

            migrationBuilder.DropColumn(
                name: "Videos",
                table: "ContentPosts");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "ContentComments");
        }
    }
}
