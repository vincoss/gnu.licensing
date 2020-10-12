using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gnu.Licensing.SqlServer.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseActivation",
                columns: table => new
                {
                    LicenseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationUuid = table.Column<Guid>(nullable: false),
                    LicenseUuid = table.Column<Guid>(nullable: false),
                    ProductUuid = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LicenseString = table.Column<string>(nullable: false),
                    LicenseAttributes = table.Column<string>(nullable: true),
                    LicenseChecksum = table.Column<string>(nullable: false),
                    AttributesChecksum = table.Column<string>(nullable: true),
                    ChecksumType = table.Column<string>(nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false),
                    CreatedByUser = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseActivation", x => x.LicenseId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseCompany",
                columns: table => new
                {
                    LicenseCompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("0b83f13d-09b0-432a-b7c6-74bc366d46ed")),
                    CompanyName = table.Column<string>(nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 11, 779, DateTimeKind.Utc).AddTicks(1585)),
                    CreatedByUser = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseCompany", x => x.LicenseCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseProduct",
                columns: table => new
                {
                    LicenseProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("d8bdb8ee-909d-427c-9ded-163a17217a1a")),
                    CompanyId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: false),
                    SignKeyName = table.Column<string>(nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 11, 781, DateTimeKind.Utc).AddTicks(9289)),
                    CreatedByUser = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseProduct", x => x.LicenseProductId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRegistration",
                columns: table => new
                {
                    LicenseRegistrationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("1cdaa326-506c-4559-9185-877c9e7f4472")),
                    ProductUuid = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LicenseName = table.Column<string>(nullable: false),
                    LicenseEmail = table.Column<string>(nullable: false),
                    LicenseType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false, defaultValue: 1),
                    ExpireUtc = table.Column<DateTime>(nullable: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 10, 12, 22, 18, 11, 787, DateTimeKind.Utc).AddTicks(3623)),
                    CreatedByUser = table.Column<string>(nullable: false)
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
