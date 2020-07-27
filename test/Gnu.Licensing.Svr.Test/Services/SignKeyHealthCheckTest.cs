using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Gnu.Licensing.Svr.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Gnu.Licensing.Svr.Services
{
    public class SignKeyHealthCheckTest
    {
        [Fact]
        public async void CheckHealthAsync()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

            var settings = new AppSettings
            {
                SignKeyPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data")
            };

            options.Value.Returns(settings);

            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var contextOptions = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(contextOptions))
                {
                    context.Database.EnsureCreated();
                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var service = new SignKeyHealthCheck(context, logger, options);

                    var ctx = new HealthCheckContext();
                    var result = await service.CheckHealthAsync(ctx);

                    Assert.Equal(HealthStatus.Healthy, result.Status);
                    Assert.Equal("The sign-key task is finished.", result.Description);
                    Assert.Equal($"Key: {product.SignKeyName}, valid.", result.Data[product.ProductName]);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void CheckHealthAsync_Error()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

            var settings = new AppSettings
            {
                SignKeyPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data")
            };

            options.Value.Returns(settings);

            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var contextOptions = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(contextOptions))
                {
                    context.Database.EnsureCreated();
                    var product = CreateProduct();
                    product.SignKeyName = "aaa";
                    context.Add(product);
                    context.SaveChanges();

                    var service = new SignKeyHealthCheck(context, logger, options);

                    var ctx = new HealthCheckContext();
                    var result = await service.CheckHealthAsync(ctx);

                    Assert.Equal(HealthStatus.Unhealthy, result.Status);
                    Assert.Equal("The sign-key task is finished.", result.Description);
                    Assert.Equal($"Key: {product.SignKeyName}, invalid or corrupted, Please generate new sign key.", result.Data[product.ProductName]);
                    Assert.NotNull(result.Exception);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private LicenseProduct CreateProduct()
        {
            return new LicenseProduct
            {
                ProductUuid = Guid.NewGuid(),
                ProductName = "test-product",
                ProductDescription = "test-description",
                SignKeyName = "CN=Gnu.Licensing",
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };
        }
    }
}
