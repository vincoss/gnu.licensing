using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NSubstitute;
using System.IO;
using Gnu.Licensing.Validation;
using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Api.Models;

namespace Gnu.Licensing.Api.Services
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

    public class LicenseServiceTest
    {
        public static string PublicKey = new StreamReader(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.public.xml")).ReadToEnd();

        [Fact]
        public async void ValidateAsync_ACT01Code_InvalidLicenseKey()
        {
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
                    var service = new LicenseService(context, logger);

                    var result = await service.ValidateAsync(new LicenseRegisterRequest());

                    Assert.Equal("ACT.01", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT02Code_InvalidProductKey()
        {
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
                    var service = new LicenseService(context, logger);
                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.02", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT01Code_LicenseRegistrationNotFound()
        {
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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = Guid.NewGuid(),
                        ProductUuid = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.01", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT02Code_InvalidProductForTheLicenseKey()
        {
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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = Guid.NewGuid()
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Equal("ACT.02", result.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT03Code_CancelledLicense()
        {
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
                    var service = new LicenseService(context, logger);
                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    context.Add(registration);
                    context.SaveChanges();

                    registration.IsActive = false;
                    context.Update(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = registration.ProductId
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
        public async void ValidateAsync_ACT04Code_ExpiredLicense()
        {
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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.ExpireUtc = DateTime.UtcNow.AddDays(-5);
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = registration.ProductId
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

        // TODO: fix that
        [Fact]
        public async void ValidateAsync_ACT05Code_ShallNotOveruseIfPurchasedSinleLicense()
        {
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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Quantity = 1;
                    context.Add(registration);
                    context.SaveChanges();

                    var licenseA = CreateLicense(registration);
                    var licenseB = CreateLicense(registration);
                    context.Add(licenseA);
                    context.Add(licenseB);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = registration.ProductId
                    };

                    var result = await service.ValidateAsync(request);

                    Assert.Null(result);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void ValidateAsync_ACT05Code_OverusedLicense()
        {
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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Quantity = 2;
                    context.Add(registration);
                    context.SaveChanges();

                    var licenseA = CreateLicense(registration);
                    var licenseB = CreateLicense(registration);
                    context.Add(licenseA);
                    context.Add(licenseB);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = registration.ProductId
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
        public async void CreateAsync_NotValid()
        {
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
                    var service = new LicenseService(context, logger);

                    var request = new LicenseRegisterRequest();

                    var result = await service.CreateAsync(request, "test-user");

                    Assert.Equal("ACT.01", result.Failure.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void CreateAsync_Throws_ACT06Code()
        {
            var logger = Substitute.For<ILogger<LicenseService>>();

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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    product.SignKeyName = ""; // Force to throw
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Quantity = 1;
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = product.LicenseProductId
                    };

                    var result = await service.CreateAsync(request, "test-user");

                    Assert.Equal("ACT.06", result.Failure.Code);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void CreateAsync()
        {
            var logger = Substitute.For<ILogger<LicenseService>>();

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
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var expireDate = DateTime.UtcNow.AddDays(1);
                    var registration = CreateRegistration(product);
                    registration.Quantity = 2;
                    registration.ExpireUtc = expireDate;
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = registration.ProductId,
                        Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    };

                    request.Attributes.Add("AppId", "app-id-0001");

                    var result = await service.CreateAsync(request, "test-user");
                    var lic = context.Licenses.ToArray();
                    var licenseRecord = await context.Licenses.SingleAsync(x => x.LicenseId == registration.LicenseRegistrationId);

                    Assert.Null(result.Failure);
                    Assert.NotNull(result.License);
                    Assert.NotNull(licenseRecord);
                    Assert.False(licenseRecord.LicenseActivationId == Guid.Empty);
                    Assert.Equal(registration.LicenseRegistrationId, licenseRecord.LicenseId);
                    Assert.Equal(registration.ProductId, licenseRecord.ProductId);
                    Assert.Equal(registration.CompanyId, licenseRecord.CompanyId);
                    Assert.NotNull(licenseRecord.LicenseString);
                    Assert.NotNull(licenseRecord.LicenseAttributes);
                    Assert.NotNull(licenseRecord.LicenseChecksum);
                    Assert.NotNull(licenseRecord.AttributesChecksum);
                    Assert.Equal(Utils.ChecksumType, licenseRecord.ChecksumType);
                    Assert.Equal(DateTime.UtcNow.ToString("yyyyMMdd"), licenseRecord.CreatedDateTimeUtc.ToString("yyyyMMdd"));
                    Assert.NotNull(licenseRecord.CreatedByUser);

                    var license = License.Load(result.License);

                    var failure = FailureStrings.Get(FailureStrings.VAL04Code);

                    var failures = license.Validate()
                                       .ExpirationDate()
                                       .When(lic => lic.Type == LicenseType.Standard)
                                       .And()
                                       .Signature()
                                       .And()
                                       .AssertThat(x => string.Equals(request.Attributes["AppId"], x.AdditionalAttributes.Get("AppId"), StringComparison.OrdinalIgnoreCase), failure)
                                       .AssertValidLicense().ToList();

                    // Valid
                    Assert.False(failures.Any());

                    // License
                    Assert.Equal(licenseRecord.LicenseActivationId, license.ActivationUuid);
                    Assert.Equal(request.LicenseUuid, license.Id);
                    Assert.Equal(registration.LicenseType, license.Type);
                    Assert.Equal(expireDate.ToString("yyyyMMdd"), license.ExpirationUtc.ToString("yyyyMMdd"));
                    Assert.Equal(2, license.Quantity);
                    Assert.Equal(registration.LicenseName, license.Customer.Name);
                    Assert.Equal(registration.LicenseEmail, license.Customer.Email);
                    Assert.Equal(registration.LicenseName, license.Customer.Company);
                    Assert.Equal(request.Attributes["AppId"], license.AdditionalAttributes.Get("AppId"));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void IsActiveAsync_NotActiveIfNotExists()
        {
            var logger = Substitute.For<ILogger<LicenseService>>();

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
                    var service = new LicenseService(context, logger);

                    var result = await service.IsActiveAsync(Guid.NewGuid());

                    Assert.False(result);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void IsActiveAsync()
        {
            var logger = Substitute.For<ILogger<LicenseService>>();

            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                License license = null;
                var contextOptions = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(contextOptions))
                {
                    context.Database.EnsureCreated();
                    var service = new LicenseService(context, logger);

                    var product = CreateProduct();
                    context.Add(product);
                    context.SaveChanges();

                    var registration = CreateRegistration(product);
                    registration.Quantity = 1;
                    context.Add(registration);
                    context.SaveChanges();

                    var request = new LicenseRegisterRequest
                    {
                        LicenseUuid = registration.LicenseRegistrationId,
                        ProductUuid = product.LicenseProductId
                    };

                    var createdResult = await service.CreateAsync(request, "test-user");
                    license = License.Load(createdResult.License);
                }

                // Create the schema in the database
                using (var context = new EfDbContext(contextOptions))
                {
                    var service = new LicenseService(context, logger);
                    var active = await service.IsActiveAsync(license.ActivationUuid);

                    Assert.True(active);
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
                LicenseProductId = Guid.NewGuid(),
                ProductName = "test-product",
                ProductDescription = "test-description",
                SignKeyName = "CN=Gnu.Licensing",
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };
        }

        private LicenseRegistration CreateRegistration(LicenseProduct product)
        {
            return new LicenseRegistration
            {
                LicenseRegistrationId = Guid.NewGuid(),
                ProductId = product.LicenseProductId,
                LicenseName = "test-name",
                LicenseEmail = "test-email",
                LicenseType = LicenseType.Standard,
                IsActive = true,
                Quantity = 1,
                ExpireUtc = null,
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "test-user"
            };
        }

        private LicenseActivation CreateLicense(LicenseRegistration registration)
        {
            return new LicenseActivation
            {
                LicenseId = registration.LicenseRegistrationId,
                ProductId = registration.ProductId,
                LicenseString = "license",
                LicenseChecksum = "checksum",
                ChecksumType = "sha256",
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = "created-user",
            };
        }

    }
}