using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Gnu.Licensing.Core
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDbContextProvider<TContext>(
            this IServiceCollection services,
            string databaseType,
            Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
            where TContext : DbContext
        {
            services.TryAddScoped<DbContext>(provider => provider.GetRequiredService<TContext>());

            return services;
        }
    }
}
