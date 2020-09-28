using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;


namespace Gnu.Licensing.Core
{
    public static partial class DependencyInjectionExtensions
    {
        private static readonly string DatabaseTypeKey = $"{nameof(LicensingOptions.Database)}:{nameof(DatabaseOptions.Type)}";

        public static IServiceCollection AddDbContextProvider<TContext>(
          this IServiceCollection services,
          string databaseType,
          Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
          where TContext : DbContext, IContext
        {
            services.TryAddScoped<IContext>(provider => provider.GetRequiredService<TContext>());
            services.AddDbContext<TContext>(configureContext);
            services.AddProvider<IContext>((provider, config) =>
            {
                if (!config.HasDatabaseType(databaseType))
                {
                    return null;
                }
                return provider.GetRequiredService<TContext>();
            });

            return services;
        }

        public static IServiceCollection AddProvider<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, IConfiguration, TService> func)
            where TService : class
        {
            services.AddSingleton<IProvider<TService>>(new DelegateProvider<TService>(func));

            return services;
        }

        public static bool HasDatabaseType(this IConfiguration config, string value)
        {
            return config[DatabaseTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        public static TService GetServiceFromProviders<TService>(IServiceProvider services) where TService : class
        {
            var providers = services.GetRequiredService<IEnumerable<IProvider<TService>>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            foreach (var provider in providers)
            {
                var result = provider.GetOrNull(services, configuration);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static IServiceCollection AddLicensingOptions<TOptions>(
            this IServiceCollection services,
            string key = null)
            where TOptions : class
        {
            services.AddSingleton<IValidateOptions<TOptions>>(new ValidateLicensingOptions<TOptions>(key));
            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                if (key != null)
                {
                    config = config.GetSection(key);
                }

                return new BindOptions<TOptions>(config);
            });

            return services;
        }
    }

    interface IProvider<TService>
    {
        TService GetOrNull(IServiceProvider provider, IConfiguration configuration);
    }

    class DelegateProvider<TService> : IProvider<TService>
    {
        private readonly Func<IServiceProvider, IConfiguration, TService> _func;

        public DelegateProvider(Func<IServiceProvider, IConfiguration, TService> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public TService GetOrNull(IServiceProvider provider, IConfiguration configuration)
        {
            return _func(provider, configuration);
        }
    }
}
