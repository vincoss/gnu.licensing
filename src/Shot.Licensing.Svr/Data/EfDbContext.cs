using Microsoft.EntityFrameworkCore;
using Shot.Licensing.Svr.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shot.Licensing.Svr.Data
{
    public class EfDbContext : DbContext
    { 
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<LicenseProduct> Products { get; set; }

        public DbSet<LicenseRegistration> Registrations { get; set; }

        public DbSet<License> Licenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite(@"Data Source=C:\Temp\Glut\Shot.Licensing.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LicenseProductConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseConfiguration());
        }
    }
}
