using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shot.Licensing.Api.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "License",
                columns: table => new
                {
                    LicenseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenseRegistrationId = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    LicenseString = table.Column<string>(type: "NVARCHAR COLLATE NOCASE", nullable: false),
                    Checksum = table.Column<string>(type: "VARCHAR(64) COLLATE NOCASE", nullable: false),
                    ChecksumType = table.Column<string>(type: "VARCHAR(12) COLLATE NOCASE", nullable: false),
                    IsActive = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ModifiedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false),
                    ModifiedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.LicenseId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseProduct",
                columns: table => new
                {
                    LicenseProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    ProductDescription = table.Column<string>(type: "NVARCHAR(1024) COLLATE NOCASE", nullable: false),
                    SignKeyName = table.Column<string>(type: "VARCHAR(64) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseProduct", x => x.LicenseProductId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRegistration",
                columns: table => new
                {
                    LicenseRegistrationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenseProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    LicenseName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseEmail = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: true),
                    Quantity = table.Column<int>(nullable: false),
                    Expire = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRegistration", x => x.LicenseRegistrationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_License_LicenseUuid",
                table: "License",
                column: "LicenseUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_License_LicenseId_IsActive",
                table: "License",
                columns: new[] { "LicenseId", "IsActive" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_ProductName",
                table: "LicenseProduct",
                column: "ProductName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_ProductUuid",
                table: "LicenseProduct",
                column: "ProductUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_LicenseProductId_ProductName",
                table: "LicenseProduct",
                columns: new[] { "LicenseProductId", "ProductName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRegistration_LicenseUuid",
                table: "LicenseRegistration",
                column: "LicenseUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRegistration_LicenseRegistrationId_LicenseProductId_LicenseName_LicenseEmail_IsActive",
                table: "LicenseRegistration",
                columns: new[] { "LicenseRegistrationId", "LicenseProductId", "LicenseName", "LicenseEmail", "IsActive" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "License");

            migrationBuilder.DropTable(
                name: "LicenseProduct");

            migrationBuilder.DropTable(
                name: "LicenseRegistration");
        }
    }
}
