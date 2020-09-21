using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Gnu.Licensing.Svr.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Gnu.Licensing.Svr
{
    /// <summary>
    /// https://localhost/api/license
    /// https://localhost/hc
    /// https://localhost/swagger/v1/swagger.json
    /// https://localhost/swagger
    /// </summary>
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(configuration, args).Build();

                if (!host.ValidateStartupOptions())
                {
                    return 1;
                }

                var app = new CommandLineApplication
                {
                    Name = "Gnu.Licensing",
                    Description = "Gnu.Licensing service",
                };

                app.Option("--urls", "The URLs that should bind to.", CommandOptionType.SingleValue);

                app.OnExecute(async () =>
                {
                    await host.RunMigrationsAsync(default);

                    Log.Information("Starting web host ({ApplicationContext})...", AppName);
                    await host.RunAsync(default);
                    return 0;
                });

                app.Execute(args);

                Log.Information("Started web host ({ApplicationContext})...", AppName);

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                      .CaptureStartupErrors(false)
                      .UseStartup<Startup>()
                      .UseContentRoot(AppContext.BaseDirectory)
                      .UseUrls("https://*:443")
                      .UseConfiguration(configuration)
                      .UseSerilog();
                })
                 .ConfigureHostConfiguration((cfg) =>
                 {
                     cfg.AddConfiguration(configuration);
                 })
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    cfg.AddConfiguration(configuration);
                });

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logFilePath = configuration["Serilog:FileLogPath"];

            var cfg = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, shared: true)
                .ReadFrom.Configuration(configuration);

            return cfg.CreateLogger();
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            return builder.Build();
        }
    }
}
