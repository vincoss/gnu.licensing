using Gnu.Licensing.Api.Interface;
using Gnu.Licensing.Api.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gnu.Licensing.Validation;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Gnu.Licensing.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Gnu.Licensing.Api.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly IContext _context;
        private readonly ILogger<LicenseService> _logger;

        public LicenseService(IContext context, ILogger<LicenseService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _context = context;
            _logger = logger;
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
                _logger.LogDebug(JsonSerializer.Serialize(request));

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

                var activationId = Guid.NewGuid();
                var product = _context.Products.Single(x => x.ProductUuid == request.ProductUuid);
                var registration = _context.Registrations.Single(x => x.LicenseUuid == request.LicenseUuid);
                var licenseString = await CreateLicenseAsync(request, registration, product, activationId);

                await CreateLicenseRecordAsync(registration, licenseString, attributesJson, attributesChecksum, userName, activationId);

                var result = new LicenseRegisterResult();
                result.License = licenseString;

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

        private Task<string> CreateLicenseAsync(LicenseRegisterRequest request, LicenseRegistration registration, LicenseProduct product, Guid activationId)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var license = License.New()
                    .WithUniqueIdentifier(registration.LicenseUuid)
                    .WithActivationId(activationId)
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

        private async Task<int> CreateLicenseRecordAsync(LicenseRegistration registration, string licenseString, string attributesJson, string attributesChecksum, string userName, Guid activationId)
        {
            var license = new LicenseActivation
            {
                ActivationUuid = activationId,
                LicenseUuid = registration.LicenseUuid,
                ProductUuid = registration.ProductUuid,
                CompanyId = registration.CompanyId,
                LicenseString = licenseString,
                LicenseAttributes = attributesJson,
                AttributesChecksum = attributesChecksum,
                LicenseChecksum = Utils.GetSha256HashFromString(licenseString),
                ChecksumType = Utils.ChecksumType,
                CreatedDateTimeUtc = DateTime.UtcNow,
                CreatedByUser = userName
            };

            _context.Licenses.Add(license);
            return await _context.SaveChangesAsync(default);
        }

        #region Private methods

        private int LicenseGetUsage(Guid licenseId)
        {
            return _context.Licenses.Count(x => x.LicenseUuid == licenseId);
        }

        public async  Task<bool> IsActiveAsync(Guid activationId)
        {
            if (activationId == Guid.Empty)
            {
                return false;
            }

            var result = await _context.Licenses.AnyAsync(x => x.ActivationUuid == activationId);

            return result;
        }

        #endregion
    }
}
