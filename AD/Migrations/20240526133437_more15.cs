using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_WorkerSubdivision_workerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerSubdivision_ADOU_ADOUid",
                table: "WorkerSubdivision");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerSubdivision_GoogleOU_GoogleOUid",
                table: "WorkerSubdivision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerSubdivision",
                table: "WorkerSubdivision");

            migrationBuilder.RenameTable(
                name: "WorkerSubdivision",
                newName: "workerSubdivisions");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerSubdivision_GoogleOUid",
                table: "workerSubdivisions",
                newName: "IX_workerSubdivisions_GoogleOUid");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerSubdivision_ADOUid",
                table: "workerSubdivisions",
                newName: "IX_workerSubdivisions_ADOUid");

            migrationBuilder.AlterColumn<int>(
                name: "typeName",
                table: "workerSubdivisions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workerSubdivisions",
                table: "workerSubdivisions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_workerSubdivisionid",
                table: "UserAccountNames",
                column: "workerSubdivisionid",
                principalTable: "workerSubdivisions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_workerSubdivisions_ADOU_ADOUid",
                table: "workerSubdivisions",
                column: "ADOUid",
                principalTable: "ADOU",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_workerSubdivisions_GoogleOU_GoogleOUid",
                table: "workerSubdivisions",
                column: "GoogleOUid",
                principalTable: "GoogleOU",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_workerSubdivisions_workerSubdivisionid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_workerSubdivisions_ADOU_ADOUid",
                table: "workerSubdivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_workerSubdivisions_GoogleOU_GoogleOUid",
                table: "workerSubdivisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workerSubdivisions",
                table: "workerSubdivisions");

            migrationBuilder.RenameTable(
                name: "workerSubdivisions",
                newName: "WorkerSubdivision");

            migrationBuilder.RenameIndex(
                name: "IX_workerSubdivisions_GoogleOUid",
                table: "WorkerSubdivision",
                newName: "IX_WorkerSubdivision_GoogleOUid");

            migrationBuilder.RenameIndex(
                name: "IX_workerSubdivisions_ADOUid",
                table: "WorkerSubdivision",
                newName: "IX_WorkerSubdivision_ADOUid");

            migrationBuilder.AlterColumn<int>(
                name: "typeName",
                table: "WorkerSubdivision",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerSubdivision",
                table: "WorkerSubdivision",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_WorkerSubdivision_workerSubdivisionid",
                table: "UserAccountNames",
                column: "workerSubdivisionid",
                principalTable: "WorkerSubdivision",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerSubdivision_ADOU_ADOUid",
                table: "WorkerSubdivision",
                column: "ADOUid",
                principalTable: "ADOU",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerSubdivision_GoogleOU_GoogleOUid",
                table: "WorkerSubdivision",
                column: "GoogleOUid",
                principalTable: "GoogleOU",
                principalColumn: "id");
        }
    }
}
