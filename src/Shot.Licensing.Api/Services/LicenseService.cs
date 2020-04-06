
using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shot.Licensing.Validation;
using Shot.Licensing.Api.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Shot.Licensing.Api.Services
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
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT03Code));
            }
            if (request.ProductId == Guid.Empty)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT05Code));
            }

            var registration = _context.Registrations.SingleOrDefault(x => x.LicenseUuid == request.LicenseId);

            if (registration == null)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT03Code));
            }

            if (registration.ProductUuid != request.ProductId)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT05Code));
            }

            if (registration.IsActive == null || registration.IsActive.Value == false)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT04Code));
            }

            if (registration.Expire != null && registration.Expire <= DateTime.UtcNow)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT07Code));
            }

            if (LicenseGetUsage(request.LicenseId) >= registration.Quantity)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT10Code));
            }
            if (IsLicenseAlreadyActivated(request.LicenseId))
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT06Code));
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

                var product = _context.Products.Single(x => x.ProductUuid == request.ProductId);
                var registration = _context.Registrations.Single(x => x.LicenseUuid == request.LicenseId);
                var str = await CreateLicense(request, registration, product);

                await CreateLicenseRecord(registration, str, userName);

                var result = new LicenseRegisterResult();
                result.License = str;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new LicenseRegisterResult
                {
                    Failure = (GeneralValidationFailure)FailureStrings.Get(FailureStrings.ACT11Code)
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
                     .LicensedTo(registration.LicenseName, registration.LicenseEmail)
                     .WithAdditionalAttributes(request.Attributes != null ? request.Attributes : new Dictionary<string, string>())
                     .CreateAndSignWithPrivateKey(key);

                return license.ToString();
            });

            return task;
        }

        private async Task<int> CreateLicenseRecord(LicenseRegistration registration, string str, string userName)
        {
            var license = new Shot.Licensing.Api.Data.License
            {
                LicenseRegistrationId = registration.LicenseRegistrationId,
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                LicenseString = str,
                Checksum = Utils.GetSha256HashFromString(str),
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

        private bool IsLicenseAlreadyActivated(Guid licenseId)
        {
            return _context.Licenses.Any(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }

        private int LicenseGetUsage(Guid licenseId)
        {
            return _context.Licenses.Count(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }

        #endregion
    }
}
