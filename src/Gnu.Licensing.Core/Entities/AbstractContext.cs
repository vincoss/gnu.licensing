using Gnu.Licensing.Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Gnu.Licensing.Core.Entities
{
    public abstract class AbstractContext<TContext> : DbContext, IContext where TContext : DbContext
    {
        public AbstractContext(DbContextOptions<TContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<LicenseCompany> Companies { get; set; }

        public DbSet<LicenseProduct> Products { get; set; }

        public DbSet<LicenseRegistration> Registrations { get; set; }

        public DbSet<LicenseActivation> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LicenseCompanyConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseProductConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseActivationConfiguration());
        }

        public abstract bool IsUniqueConstraintViolationException(DbUpdateException exception);
    }
}
