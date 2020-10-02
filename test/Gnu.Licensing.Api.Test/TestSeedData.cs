using Gnu.Licensing.Svr.Data;
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

            var product = new LicenseProduct
            {
                ProductUuid = Guid.NewGuid(),
                ProductName = "Gnu.Licensing.Svr",
                ProductDescription = "Gnu.Licensing.Svr description",
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
                LicenseName = "test-name",
                LicenseEmail = "test@example.com",
                LicenseType = LicenseType.Standard,
                IsActive = true,
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
            await context.SaveChangesAsync(CancellationToken.None);
        }
       


    }
}
