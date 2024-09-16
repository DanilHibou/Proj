using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerType");

            migrationBuilder.DropColumn(
                name: "ADExist",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "ADRequired",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ADEntity");

            migrationBuilder.AlterColumn<int>(
                name: "workerType",
                table: "UserAccountNames",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedAD",
                table: "ADEntity",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserAccountNamesid",
                table: "ADEntity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoogleOU",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OUPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OUName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleOU", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADEntity_UserAccountNamesid",
                table: "ADEntity",
                column: "UserAccountNamesid",
                unique: true,
                filter: "[UserAccountNamesid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ADEntity_UserAccountNames_UserAccountNamesid",
                table: "ADEntity",
                column: "UserAccountNamesid",
                principalTable: "UserAccountNames",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ADEntity_UserAccountNames_UserAccountNamesid",
                table: "ADEntity");

            migrationBuilder.DropTable(
                name: "GoogleOU");

            migrationBuilder.DropIndex(
                name: "IX_ADEntity_UserAccountNamesid",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "IsCreatedAD",
                table: "ADEntity");

            migrationBuilder.DropColumn(
                name: "UserAccountNamesid",
                table: "ADEntity");

            migrationBuilder.AlterColumn<int>(
                name: "workerType",
                table: "UserAccountNames",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SurName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserAccountNames",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<string>(
                name: "ADExist",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADRequired",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ADEntity",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ADEntity",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "ADEntity",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ADEntity",
                type: "nvarchar(max)",
                nullable: true);

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
        }
    }
}
