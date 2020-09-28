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
                    LicenseUuid = table.Column<Guid>(nullable: false),
                    ProductUuid = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LicenseString = table.Column<string>(nullable: false),
                    LicenseAttributes = table.Column<string>(nullable: true),
                    LicenseChecksum = table.Column<string>(nullable: false),
                    AttributesChecksum = table.Column<string>(nullable: true),
                    ChecksumType = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false),
                    ModifiedDateTimeUtc = table.Column<DateTime>(nullable: false),
                    CreatedByUser = table.Column<string>(nullable: false),
                    ModifiedByUser = table.Column<string>(nullable: false)
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
                    CompanyUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("bc994b2f-dff0-477f-a1d2-0e76c7d9319e")),
                    CompanyName = table.Column<string>(nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 9, 28, 7, 20, 58, 970, DateTimeKind.Utc).AddTicks(426)),
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
                    ProductUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("866689d0-2efe-4ca3-8792-84ce89b11e25")),
                    CompanyId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: false),
                    SignKeyName = table.Column<string>(nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 9, 28, 7, 20, 58, 971, DateTimeKind.Utc).AddTicks(9978)),
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
                    LicenseUuid = table.Column<Guid>(nullable: false, defaultValue: new Guid("001b0e8e-e6e4-4deb-87e8-c655dfb1d1d8")),
                    ProductUuid = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LicenseName = table.Column<string>(nullable: false),
                    LicenseEmail = table.Column<string>(nullable: false),
                    LicenseType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Comment = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false, defaultValue: 1),
                    ExpireUtc = table.Column<DateTime>(nullable: true),
                    CreatedDateTimeUtc = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 9, 28, 7, 20, 58, 974, DateTimeKind.Utc).AddTicks(8762)),
                    CreatedByUser = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRegistration", x => x.LicenseRegistrationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseActivation_LicenseId_IsActive",
                table: "LicenseActivation",
                columns: new[] { "LicenseId", "IsActive" },
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
