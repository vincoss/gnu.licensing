using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Gnu.Licensing.Core.Entities
{
    public interface IContext
    {
        DbSet<LicenseCompany> Companies { get; set; }

        DbSet<LicenseProduct> Products { get; set; }

        DbSet<LicenseRegistration> Registrations { get; set; }

        DbSet<LicenseActivation> Licenses { get; set; }

        bool IsUniqueConstraintViolationException(DbUpdateException exception);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
