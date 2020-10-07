using Gnu.Licensing.Core.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Services
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1
    /// </summary>
    public class SignKeyHealthCheck : IHealthCheck
    {
        private readonly IContext _context;
        private readonly ILogger<LicenseService> _logger;

        public SignKeyHealthCheck(IContext context, ILogger<LicenseService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _context = context;
            _logger = logger;
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

                    var lic = License.New()
                        .WithUniqueIdentifier(Guid.NewGuid())
                        .CreateAndSign(keyName);

                    data.Add(productName, $"Key: {keyName}, valid.");
                }

                return Task.FromResult(HealthCheckResult.Healthy(message, data));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(CheckHealthAsync));
                data.Add(productName, $"Search key: {keyName}, invalid, corrupted missing, Please install the certificate.");
                return Task.FromResult(HealthCheckResult.Unhealthy(message, ex, data));
            }
        }
    }
}
