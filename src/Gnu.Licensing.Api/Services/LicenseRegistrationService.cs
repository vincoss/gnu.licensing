using Microsoft.Extensions.Logging;
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.ViewModels;
using System;
using System.Threading.Tasks;
using Gnu.Licensing.Core.Entities;

namespace Gnu.Licensing.Svr.Services
{
    public class LicenseRegistrationService : ILicenseRegistrationService
    {
        private readonly IContext _context;
        private readonly ILogger<LicenseRegistrationService> _logger;

        public LicenseRegistrationService(IContext context, ILogger<LicenseRegistrationService> logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(LicenseRegistrationViewModel model, string createdByUser)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(createdByUser)) throw new ArgumentNullException(nameof(createdByUser));

            var registration = new LicenseRegistration
            {
                CompanyId = model.CompanyId,
                ProductUuid = model.ProductUuid,
                LicenseName = model.LicenseName,
                LicenseEmail = model.LicenseEmail,
                LicenseType = model.LicenseType,
                Quantity = model.Quantity,
                CreatedByUser = createdByUser
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync(default);

            return registration.LicenseUuid;
        }
    }
}
