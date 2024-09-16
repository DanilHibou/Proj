using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_ADOU_ADOUid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_GoogleOU_GoogleOUid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_WorkerPosition_workerPositionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "WorkerPosition");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_ADOUid",
                table: "UserAccountNames");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_GoogleOUid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "ADOUid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "GoogleOUid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "GoogleOU");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "ADOU");

            migrationBuilder.RenameColumn(
                name: "workerPositionid",
                table: "UserAccountNames",
                newName: "workerSubdivisionid");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountNames_workerPositionid",
                table: "UserAccountNames",
                newName: "IX_UserAccountNames_workerSubdivisionid");

            migrationBuilder.CreateTable(
                name: "WorkerSubdivision",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    typeName = table.Column<int>(type: "int", nullable: false),
                    ADOUid = table.Column<int>(type: "int", nullable: true),
                    GoogleOUid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerSubdivision", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkerSubdivision_ADOU_ADOUid",
                        column: x => x.ADOUid,
                        principalTable: "ADOU",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_WorkerSubdivision_GoogleOU_GoogleOUid",
                        column: x => x.GoogleOUid,
                        principalTable: "GoogleOU",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSubdivision_ADOUid",
                table: "WorkerSubdivision",
                column: "ADOUid");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSubdivision_GoogleOUid",
                table: "WorkerSubdivision",
                column: "GoogleOUid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_WorkerSubdivision_workerSubdivisionid",
                table: "UserAccountNames",
                column: "workerSubdivisionid",
                principalTable: "WorkerSubdivision",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_WorkerSubdivision_workerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "WorkerSubdivision");

            migrationBuilder.RenameColumn(
                name: "workerSubdivisionid",
                table: "UserAccountNames",
                newName: "workerPositionid");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountNames_workerSubdivisionid",
                table: "UserAccountNames",
                newName: "IX_UserAccountNames_workerPositionid");

            migrationBuilder.AddColumn<int>(
                name: "ADOUid",
                table: "UserAccountNames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoogleOUid",
                table: "UserAccountNames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "GoogleOU",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "ADOU",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkerPosition",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    typeName = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPosition", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_ADOUid",
                table: "UserAccountNames",
                column: "ADOUid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_GoogleOUid",
                table: "UserAccountNames",
                column: "GoogleOUid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_ADOU_ADOUid",
                table: "UserAccountNames",
                column: "ADOUid",
                principalTable: "ADOU",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_GoogleOU_GoogleOUid",
                table: "UserAccountNames",
                column: "GoogleOUid",
                principalTable: "GoogleOU",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_WorkerPosition_workerPositionid",
                table: "UserAccountNames",
                column: "workerPositionid",
                principalTable: "WorkerPosition",
                principalColumn: "id");
        }
    }
}
