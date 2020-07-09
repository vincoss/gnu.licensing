using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xunit;

namespace Gnu.Licensing.Svr.Data
{
    public class EfDbContextTest
    {
        [Fact]
        public async void Products_MemoeryTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(options))
                {
                    context.Database.EnsureCreated();
                    new TestSeedData().Initialize(context);

                    var products = await context.Products.ToListAsync();
                    var registrations = await context.Registrations.ToListAsync();
                    var licenses = await context.Licenses.ToListAsync();

                    Assert.True(products.Count > 0);
                    Assert.True(registrations.Count > 0);
                    Assert.True(licenses.Count > 0);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void Products_FileTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(Products_FileTest)}.db3");

            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection($"DataSource={path}");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(options))
                {
                    context.Database.EnsureCreated();
                    new TestSeedData().Initialize(context);

                    var products = await context.Products.ToListAsync();
                    var registrations = await context.Registrations.ToListAsync();
                    var licenses = await context.Licenses.ToListAsync();

                    Assert.True(products.Count > 0);
                    Assert.True(registrations.Count > 0);
                    Assert.True(licenses.Count > 0);
                }
            }
            finally
            {
                connection.Close();
                File.Delete(path);
            }
        }
    }
}
