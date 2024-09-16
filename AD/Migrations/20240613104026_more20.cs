using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserKey",
                table: "UserAccountNames",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "UserAccountNames");
        }
    }
}
