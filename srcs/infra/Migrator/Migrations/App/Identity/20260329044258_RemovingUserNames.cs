using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datntdev.Microservice.Infra.Migrator.Migrations.App.Identity
{
    /// <inheritdoc />
    public partial class RemovingUserNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AppIdentities");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AppIdentities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AppIdentities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AppIdentities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
