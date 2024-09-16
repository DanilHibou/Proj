using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADEntity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADExist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADEntity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DomainController",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainControllerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LdapFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainController", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleDomain",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleDomain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountNames",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerTypeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountNames", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserAccountNames_WorkerType_WorkerTypeid",
                        column: x => x.WorkerTypeid,
                        principalTable: "WorkerType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_WorkerTypeid",
                table: "UserAccountNames",
                column: "WorkerTypeid",
                unique: true,
                filter: "[WorkerTypeid] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADEntity");

            migrationBuilder.DropTable(
                name: "DomainController");

            migrationBuilder.DropTable(
                name: "GoogleDomain");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "WorkerType");
        }
    }
}
