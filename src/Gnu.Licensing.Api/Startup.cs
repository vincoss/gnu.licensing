using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Gnu.Licensing.Api.Infrastructure.Filters;
using Gnu.Licensing.Api.Services;
using Microsoft.Extensions.Hosting;
using Gnu.Licensing.Core.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Gnu.Licensing.Core;
using Gnu.Licensing.Core.Entities;
using Gnu.Licensing.Sqlite;
using Gnu.Licensing.SqlServer;
using Gnu.Licensing.Api.Interface;

namespace Gnu.Licensing.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .Services
            .AddHealthChecks(Configuration)
            .AddHttpClientServices(Configuration)
            .AddCustomMvc(Configuration);

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = Configuration.Get<ApplicationOptions>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseForwardedHeaders();
            app.UsePathBase(options.PathBase);
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gnu.Licensing.Api V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });//.RequireAuthorization(); // TODO: this require authorization which is not there??
            });
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddCheck<SignKeyHealthCheck>(name: "license-sign-key-check", tags: new[] { "sign-key" })
                .AddDatabaseHealthCheck(configuration)
                .AddUrlGroup(new Uri(configuration["SvrUrlHc"]), name: "gnu-licensing-api-check", tags: new string[] { "gnu-licensing-api" });

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ILicenseService, LicenseService>();
            services.AddScoped<SignKeyHealthCheck>();

            services.AddLicensingOptions<ApplicationOptions>();
            services.AddLicensingOptions<DatabaseOptions>(nameof(ApplicationOptions.Database));


            services.AddSqliteDatabase();
            services.AddSqlServerDatabase();

            services.AddScoped(DependencyInjectionExtensions.GetServiceFromProviders<IContext>);

            services.TryAddSingleton<ValidateStartupOptions>();


            return services;
        }

        public static IHealthChecksBuilder AddDatabaseHealthCheck(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var type = configuration.GetValue<string>("Database:Type");

            if(type == DatabaseOptions.Sqlite)
            {
                builder.AddSqlite(name: DatabaseOptions.Sqlite, sqliteConnectionString: configuration["Database:ConnectionString"], tags: new string[] { "sqlite" });
            }
            if (type == DatabaseOptions.SqlServer)
            {
                builder.AddSqlServer(name: DatabaseOptions.SqlServer, connectionString: configuration["Database:ConnectionString"], tags: new string[] { "sqlserver" });
            }

            return builder;
        }

    }
}

