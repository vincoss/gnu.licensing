using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Models;
using Shot.Licensing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shot.Licensing.Validation;
using Shot.Licensing.Api.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shot.Licensing.Api.Services
{
    public class LicenseService : ILicenseService
    {
        public static string PrivateKey;
        static LicenseService()
        {
            PrivateKey = File.ReadAllText(@"C:\_Dev\GitHub\FL\shot.licensing\resources\test.private.xml"); // TODO:
        }

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

            if (registration.Expire <= DateTime.UtcNow)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT07Code));
            }

            if (IsLicenseAlreadyActivated(request.LicenseId))
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT06Code));
            }

            if (LicenseGetUsage(request.LicenseId) >= registration.Quantity)
            {
                return Task.FromResult(FailureStrings.Get(FailureStrings.ACT10Code));
            }

            // ACT06Code
            return Task.FromResult<IValidationFailure>(null);
        }

        public bool IsValidLicenseId(Guid licenseId)
        {
            return _context.Registrations.Any(x => x.LicenseUuid == licenseId);
        }

        public bool IsValidLicenseProductId(Guid licenseId, Guid productId)
        {
            return _context.Registrations.Any(x => x.LicenseUuid == licenseId && x.ProductUuid == productId);
        }

        public bool IsLicenseCancelled(Guid licenseId)
        {
            return _context.Registrations.Any(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive == false);
        }

        public bool IsLicenseAlreadyActivated(Guid licenseId)
        {
            return _context.Licenses.Any(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }

        public int LicenseGetUsage(Guid licenseId)
        {
            return _context.Licenses.Count(x => x.LicenseUuid == licenseId && x.IsActive != null && x.IsActive.Value);
        }


        public async Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var failure = await ValidateAsync(request);
                if (failure != null)
                {
                    return new LicenseRegisterResult
                    {
                        Failure = failure
                    };
                }

                var str = await Create(request, null, null);

                var result = new LicenseRegisterResult();
                result.License = str;
                result.Failure = new GeneralValidationFailure
                {
                    Message = nameof(GeneralValidationFailure.Message),
                    HowToResolve = nameof(GeneralValidationFailure.HowToResolve)
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new LicenseRegisterResult
                {
                    Failure = FailureStrings.Get(FailureStrings.ACT11Code)
                };
            }
        }

        private Task<string> Create(LicenseRegisterRequest request, LicenseRegistration registration, LicenseProduct product)
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

        private string SignKeyGet(string name)
        {
            var path = Path.Combine(_options.Value.SignKeyPath, name);
            return File.ReadAllText(path);
        }
    }
}
