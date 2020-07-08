using Gnu.Licensing.Api.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Gnu.Licensing.Api
{
    public class TestSeedData
    {
        public void Initialize(EfDbContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var createdDate = DateTime.UtcNow;
            var user = Environment.UserName;

            var product = new LicenseProduct
            {
                ProductUuid = Guid.NewGuid(),
                ProductName = "Gnu.Licensing.Svr",
                ProductDescription = "Gnu.Licensing.Svr description",
                SignKeyName = "test.private.xml",
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user
            };

            context.Products.AddRange(product);
            context.SaveChanges();

            var registration = new LicenseRegistration
            {
                LicenseUuid = Guid.NewGuid(),
                ProductUuid = product.ProductUuid,
                LicenseName = "test-name",
                LicenseEmail = "test@example.com",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Quantity = 1,
                Expire = null,
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = user
            };

            context.Registrations.AddRange(registration);
            context.SaveChanges();

            var license = new Gnu.Licensing.Api.Data.License
            {
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                LicenseString = "license-string",
                LicenseChecksum = "checksum",
                ChecksumType = "sha256",
                IsActive = true,
                CreatedDateTimeUtc = createdDate,
                ModifiedDateTimeUtc = createdDate,
                CreatedByUser = user,
                ModifiedByUser = user
            };

            context.Licenses.AddRange(license);
            context.SaveChanges();
        }
       


    }
}
