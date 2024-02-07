using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsersEmployees",
                table: "ApplicationUsersEmployees");

            migrationBuilder.RenameTable(
                name: "ApplicationUsersEmployees",
                newName: "ApplicationUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "ApplicationUsersEmployees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsersEmployees",
                table: "ApplicationUsersEmployees",
                column: "Id");
        }
    }
}
