using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class mor10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAllowed",
                table: "AllowedUsers");

            migrationBuilder.CreateTable(
                name: "GoogleGroups",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Groupid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleGroups", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleGroups");

            migrationBuilder.AddColumn<bool>(
                name: "isAllowed",
                table: "AllowedUsers",
                type: "bit",
                nullable: true);
        }
    }
}
