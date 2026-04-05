using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenant.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantLocales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultLocale",
                schema: "tenant",
                table: "Tenants",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "nl");

            migrationBuilder.AddColumn<string>(
                name: "SupportedLocales",
                schema: "tenant",
                table: "Tenants",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultLocale",
                schema: "tenant",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SupportedLocales",
                schema: "tenant",
                table: "Tenants");
        }
    }
}
