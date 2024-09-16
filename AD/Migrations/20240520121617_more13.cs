using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Location",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "workerPositionid",
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
                table: "GoogleGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "ADOU",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "ADGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationUserAccountNames",
                columns: table => new
                {
                    UserAccountNamesid = table.Column<int>(type: "int", nullable: false),
                    locationid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationUserAccountNames", x => new { x.UserAccountNamesid, x.locationid });
                    table.ForeignKey(
                        name: "FK_LocationUserAccountNames_Location_locationid",
                        column: x => x.locationid,
                        principalTable: "Location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationUserAccountNames_UserAccountNames_UserAccountNamesid",
                        column: x => x.UserAccountNamesid,
                        principalTable: "UserAccountNames",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_UserAccountNames_workerPositionid",
                table: "UserAccountNames",
                column: "workerPositionid");

            migrationBuilder.CreateIndex(
                name: "IX_LocationUserAccountNames_locationid",
                table: "LocationUserAccountNames",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_WorkerPosition_workerPositionid",
                table: "UserAccountNames",
                column: "workerPositionid",
                principalTable: "WorkerPosition",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_WorkerPosition_workerPositionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "LocationUserAccountNames");

            migrationBuilder.DropTable(
                name: "WorkerPosition");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_workerPositionid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "workerPositionid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "GoogleOU");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "GoogleGroups");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "ADOU");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "ADGroups");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Location",
                newName: "Id");
        }
    }
}
