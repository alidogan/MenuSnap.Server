using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Translations",
                schema: "catalog",
                table: "ItemModifiers",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Translations",
                schema: "catalog",
                table: "ItemModifierGroups",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Translations",
                schema: "catalog",
                table: "CatalogItems",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Translations",
                schema: "catalog",
                table: "CatalogGroups",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Translations",
                schema: "catalog",
                table: "CatalogCategories",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Translations",
                schema: "catalog",
                table: "ItemModifiers");

            migrationBuilder.DropColumn(
                name: "Translations",
                schema: "catalog",
                table: "ItemModifierGroups");

            migrationBuilder.DropColumn(
                name: "Translations",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "Translations",
                schema: "catalog",
                table: "CatalogGroups");

            migrationBuilder.DropColumn(
                name: "Translations",
                schema: "catalog",
                table: "CatalogCategories");
        }
    }
}
