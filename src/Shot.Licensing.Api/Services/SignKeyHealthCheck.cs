using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shot.Licensing.Api.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Services
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1
    /// </summary>
    public class SignKeyHealthCheck : IHealthCheck
    {
        private readonly EfDbContext _context;
        private readonly ILogger<LicenseService> _logger;
        private readonly IOptionsSnapshot<AppSettings> _options;

        public SignKeyHealthCheck(EfDbContext context, ILogger<LicenseService> logger, IOptionsSnapshot<AppSettings> options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _context = context;
            _logger = logger;
            _options = options;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var products = _context.Products;
            var data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var message = "The sign-key task is finished.";
            var productName = "";
            var keyName = "";

            try
            {
                foreach (var product in products)
                {
                    productName = product.ProductName;
                    keyName = product.SignKeyName;

                    var path = Path.Combine(_options.Value.SignKeyPath, keyName);
                    var key = File.ReadAllText(path);

                    var lic = License.New()
                        .WithUniqueIdentifier(Guid.NewGuid())
                        .CreateAndSignWithPrivateKey(key);

                    data.Add(productName, $"Key: {keyName}, valid.");
                }

                return Task.FromResult(HealthCheckResult.Healthy(message, data));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(CheckHealthAsync));
                data.Add(productName, $"Key: {keyName}, invalid or corrupted, Please generate new sign key.");
                return Task.FromResult(HealthCheckResult.Unhealthy(message, ex, data));
            }
        }
    }
}
