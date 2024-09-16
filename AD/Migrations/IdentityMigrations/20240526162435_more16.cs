using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AD.Migrations.IdentityMigrations
{
    /// <inheritdoc />
    public partial class more16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessTokenIssued",
                table: "GoogleOauthTokens");

            migrationBuilder.CreateTable(
                name: "logInOutLogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logInOutLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_logInOutLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_logInOutLogs_UserId",
                table: "logInOutLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logInOutLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccessTokenIssued",
                table: "GoogleOauthTokens",
                type: "datetime2",
                nullable: true);
        }
    }
}
