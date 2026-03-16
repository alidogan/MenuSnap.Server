using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceUnit.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialServiceUnitSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "serviceunit");

            migrationBuilder.CreateTable(
                name: "ServiceUnits",
                schema: "serviceunit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    GroupName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExternalReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUnits_LocationId",
                schema: "serviceunit",
                table: "ServiceUnits",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUnits_LocationId_Code",
                schema: "serviceunit",
                table: "ServiceUnits",
                columns: new[] { "LocationId", "Code" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceUnits",
                schema: "serviceunit");
        }
    }
}
