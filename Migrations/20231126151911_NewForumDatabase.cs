using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_DOTNET2.Migrations
{
    /// <inheritdoc />
    public partial class NewForumDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CateName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CateId);
                });

            migrationBuilder.CreateTable(
                name: "ContentComments",
                columns: table => new
                {
                    ContentCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paragram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentComments", x => x.ContentCommentId);
                });

            migrationBuilder.CreateTable(
                name: "ContentPosts",
                columns: table => new
                {
                    ContentPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paragram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPosts", x => x.ContentPostId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    postId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentTotal = table.Column<int>(type: "int", nullable: false),
                    isSave = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CateId = table.Column<int>(type: "int", nullable: false),
                    ContentPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.postId);
                    table.ForeignKey(
                        name: "FK_Posts_Categories",
                        column: x => x.CateId,
                        principalTable: "Categories",
                        principalColumn: "CateId");
                    table.ForeignKey(
                        name: "FK_Posts_ContentPosts",
                        column: x => x.ContentPostId,
                        principalTable: "ContentPosts",
                        principalColumn: "ContentPostId");
                    table.ForeignKey(
                        name: "FK_Posts_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false),
                    ContentCommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_ContentComments",
                        column: x => x.ContentCommentId,
                        principalTable: "ContentComments",
                        principalColumn: "ContentCommentId");
                    table.ForeignKey(
                        name: "FK_Comments_Posts",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "postId");
                    table.ForeignKey(
                        name: "FK_Comments_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotiId);
                    table.ForeignKey(
                        name: "FK_Notifications_Posts",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "postId");
                    table.ForeignKey(
                        name: "FK_Notifications_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ContentCommentId",
                table: "Comments",
                column: "ContentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_postId",
                table: "Comments",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_postId",
                table: "Notifications",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CateId",
                table: "Posts",
                column: "CateId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ContentPostId",
                table: "Posts",
                column: "ContentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ContentComments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ContentPosts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
