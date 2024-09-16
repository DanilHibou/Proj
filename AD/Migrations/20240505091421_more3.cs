using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_WorkerType_WorkerTypeid",
                table: "UserAccountNames");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountNames_WorkerTypeid",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserAccountNames");

            migrationBuilder.RenameColumn(
                name: "WorkerTypeid",
                table: "UserAccountNames",
                newName: "workerType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "workerType",
                table: "UserAccountNames",
                newName: "WorkerTypeid");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserAccountNames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_WorkerTypeid",
                table: "UserAccountNames",
                column: "WorkerTypeid",
                unique: true,
                filter: "[WorkerTypeid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountNames_WorkerType_WorkerTypeid",
                table: "UserAccountNames",
                column: "WorkerTypeid",
                principalTable: "WorkerType",
                principalColumn: "id");
        }
    }
}
