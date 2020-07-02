using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Data
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
            var product = new LicenseProduct
            {
                ProductUuid = new Guid("C3F80BD7-9618-48F6-8250-65D113F9AED2"),
                ProductName = "Demo-Product-(Full License)",
                ProductDescription = "Demo-Product-Description-(Full License)",
                SignKeyName = "test.private.xml",
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };

            context.Products.Add(product);
            context.SaveChanges();

            var registration = new LicenseRegistration
            {
                LicenseUuid = new Guid("D65321D5-B0F9-477D-828A-086F30E2BF89"),
                ProductUuid = product.ProductUuid,
                LicenseName = "Demo-User",
                LicenseEmail = "Demo-User-Email",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Quantity = 2,
                Expire = null,
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };

            context.Registrations.Add(registration);
            context.SaveChanges();
        }
    }
}
