using Microsoft.EntityFrameworkCore;
using samplesl.Svr.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace samplesl.Svr.Data
{
    public class EfDbContext : DbContext
    { 
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<LicenseRegistration> LicenseRegistrations { get; set; }

        public DbSet<License> Licenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite(@"Data Source=C:\Temp\Glut\samplesl.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LicenseRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseConfiguration());
        }
    }
}
