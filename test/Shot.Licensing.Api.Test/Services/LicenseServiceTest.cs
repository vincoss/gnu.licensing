using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shot.Licensing.Api.Data;
using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;


namespace Shot.Licensing.Api.Services
{
    public class LicenseServiceTest
    {
        [Fact]
        public async void ValidateAsync_ACT03Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var result = await service.ValidateAsync(new LicenseRegisterRequest());

                    Assert.Equal("ACT.03", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT05Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);
                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.05", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT03Code_LicenseRegistrationNotFound()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = Guid.NewGuid(),
                        ProductId = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.03", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT05Code_InvalidProductForTheLicenseKey()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = registration.LicenseUuid,
                        ProductId = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.05", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT04Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);
                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.IsActive = false;
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = registration.LicenseUuid,
                        ProductId = registration.ProductUuid
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.04", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT07Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Expire = DateTime.UtcNow.AddDays(-5);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = registration.LicenseUuid,
                        ProductId = registration.ProductUuid
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.07", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT06Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    var license = CreateLicense(registration);
                    context.Add(license);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = registration.LicenseUuid,
                        ProductId = registration.ProductUuid
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.06", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT10Code()
        {
            var options = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var logger = new LoggerFactory().CreateLogger<LicenseService>();

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
                    var service = new LicenseService(context, logger, options);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Quantity = 1;
                    context.Add(registration);
                    context.SaveChanges();

                    var licenseA = CreateLicense(registration);
                    var licenseB = CreateLicense(registration);
                    licenseA.IsActive = false;
                    context.Add(licenseA);
                    context.Add(licenseB);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseId = registration.LicenseUuid,
                        ProductId = registration.ProductUuid
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.10", result.Code);
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
                SignKeyName = "test-sign-key-name",
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };
        }

        private LicenseRegistration CreateRegistration(LicenseProduct product)
        {
            return new LicenseRegistration
            {
                LicenseProductId = product.LicenseProductId,
                LicenseUuid = Guid.NewGuid(),
                ProductUuid = Guid.NewGuid(),
                LicenseName  = "test-name",
                LicenseEmail = "test-email",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Quantity = 1,
                Expire = null,
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };
        }

        private Shot.Licensing.Api.Data.License CreateLicense(LicenseRegistration registration)
        {
            return new Data.License
            {
                LicenseRegistrationId = registration.LicenseRegistrationId,
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                LicenseString = "license",
                Checksum = "checksum",
                ChecksumType = "sha256",
                IsActive = true,
                CreatedDateTimeUtc = DateTime.UtcNow,
                ModifiedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user",
                ModifiedByUser = "test-user"
            };
        }

    }
}
