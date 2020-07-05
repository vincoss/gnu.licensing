using Microsoft.EntityFrameworkCore;
using Warthog.Api.Data.Configuration;


namespace Warthog.Api.Data
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<LicenseProduct> Products { get; set; }

        public DbSet<LicenseRegistration> Registrations { get; set; }

        public DbSet<License> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LicenseProductConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseConfiguration());
        }
    }
}
