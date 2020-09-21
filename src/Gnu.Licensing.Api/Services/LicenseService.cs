
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Gnu.Licensing.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Gnu.Licensing.Core.Entities;

namespace Gnu.Licensing.Svr.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly IContext _context;
        private readonly ILogger<LicenseService> _logger;
        private readonly IOptionsSnapshot<AppSettings> _options;

        public LicenseService(IContext context, ILogger<LicenseService> logger, IOptionsSnapshot<AppSettings> options)
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

            if (request.LicenseUuid == Guid.Empty)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT01Code));
            }
            if (request.ProductUuid == Guid.Empty)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT02Code));
            }

            var registration = _context.Registrations.SingleOrDefault(x => x.LicenseUuid == request.LicenseUuid);

            if (registration == null)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT01Code));
            }

            if (registration.ProductUuid != request.ProductUuid)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT02Code));
            }

            if (registration.IsActive == false)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT03Code));
            }

            if (registration.ExpireUtc != null && registration.ExpireUtc <= DateTime.UtcNow)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT04Code));
            }

            if (registration.Quantity > 1 && (LicenseGetUsage(request.LicenseUuid) >= registration.Quantity))
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

                var product = _context.Products.Single(x => x.ProductUuid == request.ProductUuid);
                var registration = _context.Registrations.Single(x => x.LicenseUuid == request.LicenseUuid);
                var str = await CreateLicenseAsync(request, registration, product);

                await CreateLicenseRecordAsync(registration, str, attributesJson, attributesChecksum, userName);

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

        private Task<string> CreateLicenseAsync(LicenseRegisterRequest request, LicenseRegistration registration, LicenseProduct product)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var license = License.New()
                    .WithUniqueIdentifier(registration.LicenseUuid)
                    .As(registration.LicenseType)
                    .ExpiresAt(registration.ExpireUtc == null ? DateTime.MaxValue : registration.ExpireUtc.Value)
                    .WithMaximumUtilization(registration.Quantity)
                    .LicensedTo(registration.LicenseName, registration.LicenseEmail, (c) => c.Company = registration.LicenseName)
                    .WithAdditionalAttributes(request.Attributes != null ? request.Attributes : new Dictionary<string, string>())
                    .CreateAndSign(product.SignKeyName);

                    return license.ToString();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw;
                }
            });

            return task;
        }

        private async Task<int> CreateLicenseRecordAsync(LicenseRegistration registration, string str, string attributesJson, string attributesChecksum, string userName)
        {
            var license = new LicenseActivation
            {
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                CompanyId = registration.CompanyId,
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
            return await _context.SaveChangesAsync(default);
        }

        #region Private methods

        private int LicenseGetUsage(Guid licenseId)
        {
            return _context.Licenses.Count(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }

        #endregion
    }
}
