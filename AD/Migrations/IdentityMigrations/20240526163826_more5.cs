using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations.IdentityMigrations
{
    /// <inheritdoc />
    public partial class more5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "logInOutLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "logInOutLogs");
        }
    }
}
