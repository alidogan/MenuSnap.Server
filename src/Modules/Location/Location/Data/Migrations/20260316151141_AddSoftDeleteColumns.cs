using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "location",
                table: "Locations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "location",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "location",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "location",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "location",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "location",
                table: "Locations");
        }
    }
}
