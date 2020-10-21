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
                    LicenseActivationId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    LicenseId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_LicenseActivation", x => x.LicenseActivationId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseCompany",
                columns: table => new
                {
                    LicenseCompanyId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 21, 4, 18, 9, 312, DateTimeKind.Utc).AddTicks(2518)),
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
                    LicenseProductId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    ProductDescription = table.Column<string>(type: "NVARCHAR(1024) COLLATE NOCASE", nullable: false),
                    SignKeyName = table.Column<string>(type: "VARCHAR(64) COLLATE NOCASE", nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 21, 4, 18, 9, 317, DateTimeKind.Utc).AddTicks(2329)),
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
                    LicenseRegistrationId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    ProductId = table.Column<Guid>(type: "VARCHAR(36)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "INTEGER", nullable: false),
                    LicenseName = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseEmail = table.Column<string>(type: "NVARCHAR(256) COLLATE NOCASE", nullable: false),
                    LicenseType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(1024) COLLATE NOCASE", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExpireUtc = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2020, 10, 21, 4, 18, 9, 322, DateTimeKind.Utc).AddTicks(3520)),
                    CreatedByUser = table.Column<string>(type: "NVARCHAR(64) COLLATE NOCASE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRegistration", x => x.LicenseRegistrationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseActivation_LicenseActivationId",
                table: "LicenseActivation",
                column: "LicenseActivationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCompany_CompanyName",
                table: "LicenseCompany",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseCompany_LicenseCompanyId",
                table: "LicenseCompany",
                column: "LicenseCompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_LicenseProductId",
                table: "LicenseProduct",
                column: "LicenseProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_ProductName",
                table: "LicenseProduct",
                column: "ProductName",
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
