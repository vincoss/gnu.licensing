using Gnu.Licensing.Core;
using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Core.Options;
using Gnu.Licensing.Svr.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.Threading;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Hosting
{
    public static class IHostExtensions
    {
        public static async Task RunDataSeedAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            // Run data seed if necessary.
            var options = host.Services.GetRequiredService<IOptions<ApplicationOptions>>();
            var logger = host.Services.GetRequiredService<ILogger<ContextSeed>>();

            if (options.Value.UseCustomizationData)
            {
                logger.LogInformation("Seeding database ({ApplicationContext})...", Program.AppName);

                using (var scope = host.Services.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<IContext>();
                    if (ctx != null)
                    {
                        await new ContextSeed(logger).SeedAsync(ctx, cancellationToken);
                    }
                }
            }
        }

        public static async Task RunMigrationsAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            // Run migrations if necessary.
            var options = host.Services.GetRequiredService<IOptions<ApplicationOptions>>();
            var logger = host.Services.GetRequiredService<ILogger<ContextSeed>>();

            if (options.Value.RunMigrationsAtStartup)
            {
                logger.LogInformation("Applying migrations ({ApplicationContext})...", Program.AppName);

                using (var scope = host.Services.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<IContext>();
                    if (ctx != null)
                    {
                        await ctx.RunMigrationsAsync(cancellationToken);
                    }
                }
            }
        }

        public static bool ValidateStartupOptions(this IHost host)
        {
            return host
                .Services
                .GetRequiredService<ValidateStartupOptions>()
                .Validate();
        }
    }
}
