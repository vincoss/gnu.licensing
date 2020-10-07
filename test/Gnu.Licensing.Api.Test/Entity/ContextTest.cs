using Gnu.Licensing.Core.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xunit;


namespace Gnu.Licensing.Api.Entity
{
    public class EfDbContext : AbstractContext<EfDbContext>
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
        {
            throw new NotImplementedException();
        }
    }

    public class ContextTest
    {
        [Fact]
        public async void MemoryTest()
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

                    var companies = await context.Companies.ToListAsync();
                    var products = await context.Products.ToListAsync();
                    var registrations = await context.Registrations.ToListAsync();
                    var licenses = await context.Licenses.ToListAsync();

                    Assert.True(companies.Count > 0);
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
        public async void FileTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(FileTest)}.db3");

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
