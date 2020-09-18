using Gnu.Licensing.Core;
using Gnu.Licensing.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;


namespace Gnu.Licensing.SqlServer
{
    public static class SqlServerApplicationExtensions
    {
        public static IServiceCollection AddSqlServerDatabase(this IServiceCollection services)
        {
            services.AddDbContextProvider<SqlServerContext>("SqlServer", (provider, options) =>
            {
                var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();

                options.UseSqlServer(databaseOptions.Value.ConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddSqlServerDatabase(this IServiceCollection services, Action<DatabaseOptions> configure)
        {
            services.AddSqlServerDatabase();
            services.Configure(configure);
            return services;
        }
    }
}
