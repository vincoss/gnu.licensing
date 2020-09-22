using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Gnu.Licensing.Api.Factories
{
    //public class EfDbContextFactory : IDesignTimeDbContextFactory<EfDbContext>
    //{
    //    public EfDbContext CreateDbContext(string[] args)
    //    {
    //        var config = new ConfigurationBuilder()
    //           .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
    //           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //           .AddEnvironmentVariables()
    //           .Build();

    //        var optionsBuilder = new DbContextOptionsBuilder<EfDbContext>();

    //        optionsBuilder.UseSqlite(config.GetConnectionString("EfDbContext"), sqliteOptionsAction: o => o.MigrationsAssembly("Gnu.Licensing.Api"));

    //        return new EfDbContext(optionsBuilder.Options);
    //    }
    //}
}
