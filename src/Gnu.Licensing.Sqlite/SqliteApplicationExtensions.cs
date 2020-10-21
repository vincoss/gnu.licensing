using Gnu.Licensing.Core;
using Gnu.Licensing.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;


namespace Gnu.Licensing.Sqlite
{
    public static class SqlServerApplicationExtensions
    {
        public static IServiceCollection AddSqliteDatabase(this IServiceCollection services)
        {
            services.AddDbContextProvider<SqliteContext>(DatabaseOptions.Sqlite, (provider, options) =>
            {
                var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
                options.UseSqlite(databaseOptions.Value.ConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddSqliteDatabase(this IServiceCollection services, Action<DatabaseOptions> configure)
        {
            services.AddSqliteDatabase();
            services.Configure(configure);
            return services;
        }
    }
}
