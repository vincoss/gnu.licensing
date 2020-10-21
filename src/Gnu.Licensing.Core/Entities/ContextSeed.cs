using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gnu.Licensing.Core.Entities;
using Microsoft.Extensions.Logging;


namespace Gnu.Licensing.Api.Data
{
    public class ContextSeed
    {
        private ILogger<ContextSeed> _logger;

        public ContextSeed(ILogger<ContextSeed> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SeedAsync(IContext context, CancellationToken cancellationToken, int? retry = 0)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            int retryForAvaiability = retry.Value;

            try
            {
                await DemoSeedData(context, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(IContext));

                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;
                    await SeedAsync(context, cancellationToken, retryForAvaiability);
                }
            }
        }

        private async Task DemoSeedData(IContext context, CancellationToken cancellationToken)
        {
            var userName = "test-user";
            var createdDate = DateTime.UtcNow;

            if (context.Companies.Any() == false)
            {
                _logger.LogDebug("Begin database seeding");

                var company = new LicenseCompany
                {
                    LicenseCompanyId = new Guid("A8AD667E-8DDC-4819-914F-55AA9B6CD50B"),
                    CompanyName = "Demo-Company",
                    CreatedByUser = userName,
                    CreatedDateTimeUtc = createdDate
                };
                context.Companies.Add(company);
                await context.SaveChangesAsync(cancellationToken);

                if (context.Products.Any() == false)
                {
                    var product = new LicenseProduct
                    {
                        CompanyId = company.LicenseCompanyId,
                        LicenseProductId = new Guid("C3F80BD7-9618-48F6-8250-65D113F9AED2"),
                        ProductName = "Demo-Product-(Full License)",
                        ProductDescription = "Demo-Product-Description-(Full License)",
                        SignKeyName = "CN=Gnu.Licensing",
                        CreatedDateTimeUtc = createdDate,
                        CreatedByUser = userName
                    };

                    context.Products.Add(product);
                    await context.SaveChangesAsync(cancellationToken);

                    if (context.Registrations.Any() == false)
                    {
                        var registration = new LicenseRegistration
                        {
                            LicenseRegistrationId = new Guid("D65321D5-B0F9-477D-828A-086F30E2BF89"),
                            ProductId = product.LicenseProductId,
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
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                _logger.LogDebug("End database seeding");
            }
        }
    }
}
