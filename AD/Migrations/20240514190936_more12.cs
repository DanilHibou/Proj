using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations
{
    /// <inheritdoc />
    public partial class more12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADEntity");

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

            migrationBuilder.AddColumn<bool>(
                name: "isADCreated",
                table: "UserAccountNames",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGoogleCreated",
                table: "UserAccountNames",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ADGroupsUserAccountNames",
                columns: table => new
                {
                    ADGroupsid = table.Column<int>(type: "int", nullable: false),
                    UserAccountNamesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADGroupsUserAccountNames", x => new { x.ADGroupsid, x.UserAccountNamesid });
                    table.ForeignKey(
                        name: "FK_ADGroupsUserAccountNames_ADGroups_ADGroupsid",
                        column: x => x.ADGroupsid,
                        principalTable: "ADGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ADGroupsUserAccountNames_UserAccountNames_UserAccountNamesid",
                        column: x => x.UserAccountNamesid,
                        principalTable: "UserAccountNames",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoogleGroupsUserAccountNames",
                columns: table => new
                {
                    GoogleGroupsid = table.Column<int>(type: "int", nullable: false),
                    UserAccountNamesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleGroupsUserAccountNames", x => new { x.GoogleGroupsid, x.UserAccountNamesid });
                    table.ForeignKey(
                        name: "FK_GoogleGroupsUserAccountNames_GoogleGroups_GoogleGroupsid",
                        column: x => x.GoogleGroupsid,
                        principalTable: "GoogleGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoogleGroupsUserAccountNames_UserAccountNames_UserAccountNamesid",
                        column: x => x.UserAccountNamesid,
                        principalTable: "UserAccountNames",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_ADOUid",
                table: "UserAccountNames",
                column: "ADOUid");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountNames_GoogleOUid",
                table: "UserAccountNames",
                column: "GoogleOUid");

            migrationBuilder.CreateIndex(
                name: "IX_ADGroupsUserAccountNames_UserAccountNamesid",
                table: "ADGroupsUserAccountNames",
                column: "UserAccountNamesid");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleGroupsUserAccountNames_UserAccountNamesid",
                table: "GoogleGroupsUserAccountNames",
                column: "UserAccountNamesid");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_ADOU_ADOUid",
                table: "UserAccountNames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountNames_GoogleOU_GoogleOUid",
                table: "UserAccountNames");

            migrationBuilder.DropTable(
                name: "ADGroupsUserAccountNames");

            migrationBuilder.DropTable(
                name: "GoogleGroupsUserAccountNames");

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
                name: "isADCreated",
                table: "UserAccountNames");

            migrationBuilder.DropColumn(
                name: "isGoogleCreated",
                table: "UserAccountNames");

            migrationBuilder.CreateTable(
                name: "ADEntity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountNamesid = table.Column<int>(type: "int", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCreatedAD = table.Column<bool>(type: "bit", nullable: true),
                    OU = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADEntity", x => x.id);
                    table.ForeignKey(
                        name: "FK_ADEntity_UserAccountNames_UserAccountNamesid",
                        column: x => x.UserAccountNamesid,
                        principalTable: "UserAccountNames",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADEntity_UserAccountNamesid",
                table: "ADEntity",
                column: "UserAccountNamesid",
                unique: true,
                filter: "[UserAccountNamesid] IS NOT NULL");
        }
    }
}
