using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Data
{
    public class EfDbContextSeed
    {
        public async Task SeedAsync(EfDbContext context, IWebHostEnvironment env, ILogger<EfDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (context.Products.Any() == false && settings.Value.UseCustomizationData)
                {
                    DemoSeedData(context);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(EfDbContext));

                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    await SeedAsync(context, env, logger, settings, retryForAvaiability);
                }
            }
        }

        private void DemoSeedData(EfDbContext context)
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
            context.SaveChanges();

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
            context.SaveChanges();

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
            context.SaveChanges();
        }
    }
}
