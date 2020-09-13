using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gnu.Licensing.Svr.Data;
using Gnu.Licensing.Svr.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Gnu.Licensing.Svr.Services
{
    public class LicenseRegistrationServiceTest
    {
        [Fact]
        public async void Create()
        {
            var loggerp = new LoggerFactory().CreateLogger<LicenseProductService>();
            var loggerr = new LoggerFactory().CreateLogger<LicenseRegistrationService>();

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
                    var productService = new LicenseProductService(context, loggerp);
                    var registrationService = new LicenseRegistrationService(context, loggerr);

                    var product = new LicenseProductViewModel
                    {
                        Name = "test",
                        Description = "test-desc",
                        SignKeyName = "test-key"
                    };

                    var productUuid = await productService.CreateAsync(product, Environment.UserName);

                    var registration = new LicenseRegistrationViewModel
                    {
                        ProductUuid = productUuid,
                        LicenseName = "test-name",
                        LicenseEmail = "test-email",
                        LicenseType = LicenseType.Standard,
                        Quantity = 1,
                    };

                    var licenseUuid = await registrationService.CreateAsync(registration, Environment.UserName);

                    Assert.False(productUuid == Guid.Empty);
                    Assert.False(licenseUuid == Guid.Empty);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
