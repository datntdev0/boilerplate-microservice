using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datntdev.Microservice.Infra.Migrator.Migrations.Srv.Identity
{
    /// <inheritdoc />
    public partial class AddPermissionsToUserAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "AppRoles");
        }
    }
}
