
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gnu.Licensing.Validation;
using Gnu.Licensing.Svr.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Gnu.Licensing.Svr.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly EfDbContext _context;
        private readonly ILogger<LicenseService> _logger;
        private readonly IOptionsSnapshot<AppSettings> _options;

        public LicenseService(EfDbContext context, ILogger<LicenseService> logger, IOptionsSnapshot<AppSettings> options)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if(options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _context = context;
            _logger = logger;
            _options = options;
        }

        public Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.LicenseId == Guid.Empty)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT01Code));
            }
            if (request.ProductId == Guid.Empty)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT02Code));
            }

            var registration = _context.Registrations.SingleOrDefault(x => x.LicenseUuid == request.LicenseId);

            if (registration == null)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT01Code));
            }

            if (registration.ProductUuid != request.ProductId)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT02Code));
            }

            if (registration.IsActive == false)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT03Code));
            }

            if (registration.Expire != null && registration.Expire <= DateTime.UtcNow)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT04Code));
            }

            if (registration.Quantity > 1 && LicenseGetUsage(request.LicenseId) >= registration.Quantity)   // TODO:
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT05Code));
            }

            return Task.FromResult<IValidationFailure>(null);
        }

        public async Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request, string userName)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if(string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            try
            {
                var failure = await ValidateAsync(request);
                if (failure != null)
                {
                    return new LicenseRegisterResult
                    {
                        Failure = (GeneralValidationFailure)failure
                    };
                }

                string attributesJson = null;
                string attributesChecksum = null;

                if(request.Attributes != null && request.Attributes.Any())
                {
                    attributesJson = JsonSerializer.Serialize(request.Attributes);
                    attributesChecksum = Utils.GetSha256HashFromString(attributesJson);
                }

                var product = _context.Products.Single(x => x.ProductUuid == request.ProductId);
                var registration = _context.Registrations.Single(x => x.LicenseUuid == request.LicenseId);
                var str = await CreateLicense(request, registration, product);

                await CreateLicenseRecord(registration, str, attributesJson, attributesChecksum, userName);

                var result = new LicenseRegisterResult();
                result.License = str;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new LicenseRegisterResult
                {
                    Failure = (GeneralValidationFailure)FailureStrings.Get(FailureStrings.ACT06Code)
                };
            }
        }

        private Task<string> CreateLicense(LicenseRegisterRequest request, LicenseRegistration registration, LicenseProduct product)
        {
            var task = Task.Run(() =>
            {
                var key = SignKeyGet(product.SignKeyName);

                var license = License.New()
                     .WithUniqueIdentifier(registration.LicenseUuid)
                     .As(registration.LicenseType)
                     .ExpiresAt(registration.Expire == null ? DateTime.MaxValue : registration.Expire.Value)
                     .WithMaximumUtilization(registration.Quantity)
                     .LicensedTo(registration.LicenseName, registration.LicenseEmail, (c) => c.Company = registration.LicenseName)
                     .WithAdditionalAttributes(request.Attributes != null ? request.Attributes : new Dictionary<string, string>())
                     .CreateAndSignWithPrivateKey(key);

                return license.ToString();
            });

            return task;
        }

        private async Task<int> CreateLicenseRecord(LicenseRegistration registration, string str, string attributesJson, string attributesChecksum, string userName)
        {
            var license = new Gnu.Licensing.Svr.Data.License
            {
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                LicenseString = str,
                LicenseAttributes = attributesJson,
                AttributesChecksum = attributesChecksum,
                LicenseChecksum = Utils.GetSha256HashFromString(str),
                ChecksumType = Utils.ChecksumType,
                IsActive = true,
                CreatedDateTimeUtc = DateTime.UtcNow,
                ModifiedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = userName,
                ModifiedByUser = userName
            };

            _context.Licenses.Add(license);
            return await _context.SaveChangesAsync();
        }

        private string SignKeyGet(string name)
        {
            var path = Path.Combine(_options.Value.SignKeyPath, name);
            return File.ReadAllText(path);
        }

        #region Private methods

        private int LicenseGetUsage(Guid licenseId)
        {
            return _context.Licenses.Count(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }

        #endregion
    }
}
