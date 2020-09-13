using Microsoft.Extensions.Logging;
using Gnu.Licensing.Svr.Data;
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Services
{
    public class LicenseProductService : ILicenseProductService
    {
        private readonly EfDbContext _context;
        private readonly ILogger<LicenseProductService> _logger;

        public LicenseProductService(EfDbContext context, ILogger<LicenseProductService> logger)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _context = context;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(LicenseProductViewModel model, string createdByUser)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(createdByUser)) throw new ArgumentNullException(nameof(createdByUser));

            var product = new LicenseProduct
            {
                ProductName = model.Name,
                ProductDescription = model.Description,
                SignKeyName = model.SignKeyName,
                CreatedByUser = createdByUser
            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            return product.ProductUuid;
        }
    }
}
