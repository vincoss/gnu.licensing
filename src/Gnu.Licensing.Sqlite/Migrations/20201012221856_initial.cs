using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gnu.Licensing.Sqlite.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseActivation",
                columns: table => new
                {
                    LicenseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivationUuid = table.Column<Guid>(nullable: false),
                    LicenseUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseString = table.Column<string>(type: "NVARCHAR COLLATE NOCASE", nullable: false),
                    LicenseAttributes = table.Column<string>(type: "NVARCHAR COLLATE NOCASE", nullable: true),
                    LicenseChecksum = table.Column<string>(type: "NVARCHAR", nullable: false),
                    AttributesChecksum = table.Column<string>(type: "NVARCHAR", nullable: true),
                    ChecksumType = table.Column<string>(type: "VARCHAR(12) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseActivation", x => x.LicenseId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseCompany",
                columns: table => new
                {
                    LicenseCompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false, defaultValue: new Guid("7040d84b-86b1-4ea9-9394-7b0b84fc784e")),
                    CompanyName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 56, 47, DateTimeKind.Utc).AddTicks(7343)),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseCompany", x => x.LicenseCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseProduct",
                columns: table => new
                {
                    LicenseProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false, defaultValue: new Guid("d2b71c0f-df99-4ea5-ac42-6a624362c9dc")),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    ProductDescription = table.Column<string>(type: "NVARCHAR(1024) COLLATE NOCASE", nullable: false),
                    SignKeyName = table.Column<string>(type: "VARCHAR(64) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 56, 49, DateTimeKind.Utc).AddTicks(7882)),
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
                    LicenseUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false, defaultValue: new Guid("4232a2b4-1f51-4228-befd-cc02a6d8a093")),
                    ProductUuid = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseEmail = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(1024) COLLATE NOCASE", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExpireUtc = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 56, 54, DateTimeKind.Utc).AddTicks(3302)),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRegistration", x => x.LicenseRegistrationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseActivation_LicenseId",
                table: "LicenseActivation",
                column: "LicenseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCompany_CompanyName",
                table: "LicenseCompany",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCompany_CompanyUuid",
                table: "LicenseCompany",
                column: "CompanyUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCompany_LicenseCompanyId_CompanyName",
                table: "LicenseCompany",
                columns: new[] { "LicenseCompanyId", "CompanyName" },
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
                name: "IX_LicenseRegistration_LicenseRegistrationId_LicenseName_LicenseEmail_IsActive",
                table: "LicenseRegistration",
                columns: new[] { "LicenseRegistrationId", "LicenseName", "LicenseEmail", "IsActive" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseActivation");

            migrationBuilder.DropTable(
                name: "LicenseCompany");

            migrationBuilder.DropTable(
                name: "LicenseProduct");

            migrationBuilder.DropTable(
                name: "LicenseRegistration");
        }
    }
}
