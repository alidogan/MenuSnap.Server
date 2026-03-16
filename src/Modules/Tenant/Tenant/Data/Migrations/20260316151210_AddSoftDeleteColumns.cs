using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenant.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "tenant",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "tenant",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "tenant",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "tenant",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "tenant",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "tenant",
                table: "Tenants");
        }
    }
}
