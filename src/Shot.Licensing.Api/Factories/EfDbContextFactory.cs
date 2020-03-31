using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shot.Licensing.Api.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Factories
{
    public class EfDbContextFactory : IDesignTimeDbContextFactory<EfDbContext>
    {
        public EfDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EfDbContext>();

            optionsBuilder.UseSqlite(config.GetConnectionString("EfDbContext"), sqliteOptionsAction: o => o.MigrationsAssembly("Shot.Licensing.Api"));

            return new EfDbContext(optionsBuilder.Options);
        }
    }
}
