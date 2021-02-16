using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.EntityFrameworkCore.DbMigrations.Migrations
{
    public partial class ModifyPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategotyId",
                table: "myblog_Posts",
                newName: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "myblog_Posts",
                newName: "CategotyId");
        }
    }
}
