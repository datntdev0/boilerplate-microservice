using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datntdev.Microservice.Infra.Migrator.Migrations.Srv.Identity
{
    /// <inheritdoc />
    public partial class AddUserTenantRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserTenants",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTenants", x => new { x.UserId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_AppUserTenants_AppUsers_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppUserTenants_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTenants_UserEntityId",
                table: "AppUserTenants",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTenants_UserId_TenantId",
                table: "AppUserTenants",
                columns: new[] { "UserId", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserTenants");
        }
    }
}
