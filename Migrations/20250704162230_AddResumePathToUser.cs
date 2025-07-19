using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFinder.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResumePathToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "Users");
        }
    }
}
