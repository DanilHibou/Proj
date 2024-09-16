using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_workerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "LocationUserAccountNames");

            migrationBuilder.DropColumn(
                name: "typeName",
                table: "workerSubdivisions");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "GoogleGroups");

            migrationBuilder.DropColumn(
                name: "workerType",
                table: "ADGroups");

            migrationBuilder.RenameColumn(
                name: "workerSubdivisionid",
                table: "UserAccountNames",
                newName: "WorkerSubdivisionid");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountNames_workerSubdivisionid",
                table: "UserAccountNames",
                newName: "IX_UserAccountNames_WorkerSubdivisionid");

            migrationBuilder.AddColumn<int>(
                name: "Locationid",
                table: "UserAccountNames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "organizationalDivisionsid",
                table: "UserAccountNames",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "organizationalDivisions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ADOUid = table.Column<int>(type: "int", nullable: true),
                    GoogleOUid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizationalDivisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_organizationalDivisions_ADOU_ADOUid",
                        column: x => x.ADOUid,
                        principalTable: "ADOU",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_organizationalDivisions_GoogleOU_GoogleOUid",
                        column: x => x.GoogleOUid,
                        principalTable: "GoogleOU",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_organizationalDivisions_organizationalDivisions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "organizationalDivisions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_Locationid",
                table: "UserAccountNames",
                column: "Locationid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_organizationalDivisionsid",
                table: "UserAccountNames",
                column: "organizationalDivisionsid");

            migrationBuilder.CreateIndex(
                name: "IX_organizationalDivisions_ADOUid",
                table: "organizationalDivisions",
                column: "ADOUid");

            migrationBuilder.CreateIndex(
                name: "IX_organizationalDivisions_GoogleOUid",
                table: "organizationalDivisions",
                column: "GoogleOUid");

            migrationBuilder.CreateIndex(
                name: "IX_organizationalDivisions_ParentId",
                table: "organizationalDivisions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_Location_Locationid",
                table: "UserAccountNames",
                column: "Locationid",
                principalTable: "Location",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_organizationalDivisions_organizationalDivisionsid",
                table: "UserAccountNames",
                column: "organizationalDivisionsid",
                principalTable: "organizationalDivisions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_WorkerSubdivisionid",
                table: "UserAccountNames",
                column: "WorkerSubdivisionid",
                principalTable: "workerSubdivisions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_Location_Locationid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_organizationalDivisions_organizationalDivisionsid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_WorkerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "organizationalDivisions");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_Locationid",
                table: "UserAccountNames");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_organizationalDivisionsid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "Locationid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "organizationalDivisionsid",
                table: "UserAccountNames");

            migrationBuilder.RenameColumn(
                name: "WorkerSubdivisionid",
                table: "UserAccountNames",
                newName: "workerSubdivisionid");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountNames_WorkerSubdivisionid",
                table: "UserAccountNames",
                newName: "IX_UserAccountNames_workerSubdivisionid");

            migrationBuilder.AddColumn<int>(
                name: "typeName",
                table: "workerSubdivisions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "UserAccountNames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "workerType",
                table: "GoogleGroups",
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

            migrationBuilder.CreateIndex(
                name: "IX_LocationUserAccountNames_locationid",
                table: "LocationUserAccountNames",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_workerSubdivisionid",
                table: "UserAccountNames",
                column: "workerSubdivisionid",
                principalTable: "workerSubdivisions",
                principalColumn: "id");
        }
    }
}
