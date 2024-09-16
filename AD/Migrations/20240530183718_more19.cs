using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_WorkerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "workerSubdivisions");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_WorkerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "WorkerSubdivisionid",
                table: "UserAccountNames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerSubdivisionid",
                table: "UserAccountNames",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "workerSubdivisions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADOUid = table.Column<int>(type: "int", nullable: true),
                    GoogleOUid = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workerSubdivisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_workerSubdivisions_ADOU_ADOUid",
                        column: x => x.ADOUid,
                        principalTable: "ADOU",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_workerSubdivisions_GoogleOU_GoogleOUid",
                        column: x => x.GoogleOUid,
                        principalTable: "GoogleOU",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_WorkerSubdivisionid",
                table: "UserAccountNames",
                column: "WorkerSubdivisionid");

            migrationBuilder.CreateIndex(
                name: "IX_workerSubdivisions_ADOUid",
                table: "workerSubdivisions",
                column: "ADOUid");

            migrationBuilder.CreateIndex(
                name: "IX_workerSubdivisions_GoogleOUid",
                table: "workerSubdivisions",
                column: "GoogleOUid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_WorkerSubdivisionid",
                table: "UserAccountNames",
                column: "WorkerSubdivisionid",
                principalTable: "workerSubdivisions",
                principalColumn: "id");
        }
    }
}
