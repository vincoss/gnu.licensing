using Microsoft.EntityFrameworkCore;
using Gnu.Licensing.Svr.Data.Configuration;


namespace Gnu.Licensing.Svr.Data
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<LicenseCompany> Companies { get; set; }

        public DbSet<LicenseProduct> Products { get; set; }

        public DbSet<LicenseRegistration> Registrations { get; set; }

        public DbSet<License> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LicenseCompanyConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseProductConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseConfiguration());
        }
    }
}
