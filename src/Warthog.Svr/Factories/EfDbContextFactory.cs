using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Warthog.Api.Data;
using System.IO;


namespace Warthog.Api.Factories
{
    public class EfDbContextFactory : IDesignTimeDbContextFactory<EfDbContext>
    {
        public EfDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EfDbContext>();

            optionsBuilder.UseSqlite(config.GetConnectionString("EfDbContext"), sqliteOptionsAction: o => o.MigrationsAssembly("Warthog.Api"));

            return new EfDbContext(optionsBuilder.Options);
        }
    }
}
