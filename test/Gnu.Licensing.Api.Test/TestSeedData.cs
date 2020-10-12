using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Gnu.Licensing.Core.Entities;
using System.Threading;

namespace Gnu.Licensing.Api
{
    public class TestSeedData
    {
        public async void Initialize(IContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var createdDate = DateTime.UtcNow;
            var user = Environment.UserName;

            var company = new LicenseCompany
            {
                CompanyUuid = Guid.NewGuid(),
                CompanyName = "Gnu.Licensing.Api",
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user
            };

            context.Companies.AddRange(company);
            await context.SaveChangesAsync(CancellationToken.None);

            var product = new LicenseProduct
            {
                CompanyId = company.LicenseCompanyId,
                ProductUuid = Guid.NewGuid(),
                ProductName = "Gnu.Licensing.Api",
                ProductDescription = "Gnu.Licensing.Api description",
                SignKeyName = "CN=Gnu.Licensing",
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user
            };

            context.Products.AddRange(product);
            await context.SaveChangesAsync(CancellationToken.None);

            var registration = new LicenseRegistration
            {
                LicenseUuid = Guid.NewGuid(),
                ProductUuid = product.ProductUuid,
                CompanyId = product.CompanyId,
                LicenseName = "test-name",
                LicenseEmail = "test@example.com",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Comment = "Comment",
                Quantity = 1,
                ExpireUtc = null,
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user
            };

            context.Registrations.AddRange(registration);
            await context.SaveChangesAsync(CancellationToken.None);

            var license = new LicenseActivation
            {
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                CompanyId = registration.CompanyId,
                LicenseString = "license-string",
                LicenseAttributes = "license-attributes",
                LicenseChecksum = "checksum",
                AttributesChecksum = "checksum",
                ChecksumType = "sha256",
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user,
            };

            context.Licenses.AddRange(license);
            await context.SaveChangesAsync(CancellationToken.None);
        }
       


    }
}
