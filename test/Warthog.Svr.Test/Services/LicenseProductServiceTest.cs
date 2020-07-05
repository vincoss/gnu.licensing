using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Warthog.Api.Data;
using Warthog.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Warthog.Api.Services
{
    public class LicenseProductServiceTest
    {
        [Fact]
        public async void Create()
        {
            var logger = new LoggerFactory().CreateLogger<LicenseProductService>();

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
                    var service = new LicenseProductService(context, logger);

                    var model = new LicenseProductViewModel
                    {
                        Name = "test",
                        Description = "test-desc",
                        SignKeyName = "test-key"
                    };

                    var result = await service.Create(model, Environment.UserName);

                    Assert.False(result == Guid.Empty);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
