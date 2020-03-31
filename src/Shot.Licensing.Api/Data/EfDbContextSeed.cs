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

                if (!context.Products.Any())
                {
                    // TODO: seed
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(EfDbContext));

                    await SeedAsync(context, env, logger, settings, retryForAvaiability);
                }
            }
        }
    }
}
