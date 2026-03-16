using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "catalog",
                table: "ItemModifiers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "catalog",
                table: "ItemModifiers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "catalog",
                table: "ItemModifiers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "catalog",
                table: "ItemModifierGroups",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "catalog",
                table: "ItemModifierGroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "catalog",
                table: "ItemModifierGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogGroups",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogGroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogCategories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalog",
                table: "ItemModifiers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalog",
                table: "ItemModifiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "catalog",
                table: "ItemModifiers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalog",
                table: "ItemModifierGroups");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalog",
                table: "ItemModifierGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "catalog",
                table: "ItemModifierGroups");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogGroups");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogGroups");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalog",
                table: "CatalogCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalog",
                table: "CatalogCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "catalog",
                table: "CatalogCategories");
        }
    }
}
