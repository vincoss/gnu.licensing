using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shot.Licensing.Api.Data;
using Shot.Licensing.Api.Infrastructure.Filters;
using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Services;


namespace Shot.Licensing.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                    {
                        options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                        options.Filters.Add(typeof(RestrictHttpsAttribute)); // TODO: disable for easy testing
                    })
                    .Services
                    .AddHealthChecks(Configuration)
                    .AddHttpClientServices(Configuration)
                    .AddCustomMvc(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlite(sqliteConnectionString: configuration.GetConnectionString("EfDbContext"))
                .AddUrlGroup(new Uri(configuration["SvrUrlHC"]), name: "shot-svr-check", tags: new string[] { "shot.svr" });

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AppSettings>(configuration);

            return services;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ILicenseService, LicenseService>();
            services.AddDbContext<EfDbContext>(options => options.UseSqlite(configuration.GetConnectionString("EfDbContext")), ServiceLifetime.Transient);

            return services;
        }

    }
}

