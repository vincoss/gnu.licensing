using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Data
{
    public class ContextSeed
    {
        public async Task SeedAsync(IContext context, ILogger<ContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (context.Products.Any() == false)
                {
                    DemoSeedData(context);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(IContext));

                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    await SeedAsync(context, logger, retryForAvaiability);
                }
            }
        }

        private void DemoSeedData(IContext context, CancellationToken cancellationToken = default)
        {
            var userName = "test-user";
            var createdDate = DateTime.UtcNow;

            var company = new LicenseCompany
            {
                CompanyUuid = new Guid("A8AD667E-8DDC-4819-914F-55AA9B6CD50B"),
                CompanyName = "Demo-Company",
                CreatedByUser = userName,
                CreatedDateTimeUtc = createdDate
            };
            context.Companies.Add(company);
            context.SaveChangesAsync(cancellationToken);

            var product = new LicenseProduct
            {
                CompanyId = company.LicenseCompanyId,
                ProductUuid = new Guid("C3F80BD7-9618-48F6-8250-65D113F9AED2"),
                ProductName = "Demo-Product-(Full License)",
                ProductDescription = "Demo-Product-Description-(Full License)",
                SignKeyName = "CN=Gnu.Licensing",
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = userName
            };

            context.Products.Add(product);
            context.SaveChangesAsync(cancellationToken);

            var registration = new LicenseRegistration
            {
                LicenseUuid = new Guid("D65321D5-B0F9-477D-828A-086F30E2BF89"),
                ProductUuid = product.ProductUuid,
                CompanyId = company.LicenseCompanyId,
                LicenseName = "Demo-User",
                LicenseEmail = "Demo-User-Email",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Quantity = 2,
                ExpireUtc = null,
                CreatedDateTimeUtc = createdDate,
                CreatedByUser = userName
            };

            context.Registrations.Add(registration);
            context.SaveChangesAsync(cancellationToken);
        }
    }
}
