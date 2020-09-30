using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Gnu.Licensing.Api.Hosting;
using Microsoft.Extensions.CommandLineUtils;


namespace Gnu.Licensing.Api
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
                var host = CreateHostBuilder(args).Build();

                if (!host.ValidateStartupOptions())
                {
                    return 1;
                }

                var app = new CommandLineApplication // TODO: remove ???
                {
                    Name = "Gnu.Licensing",
                    Description = "Gnu.Licensing service",
                };

                app.Option("--urls", "The URLs that should bind to.", CommandOptionType.SingleValue);

                app.OnExecute(async () =>
                {
                    await host.RunMigrationsAsync();
                    await host.RunDataSeedAsync();

                    Log.Information("Starting web host ({ApplicationContext})...", AppName);
                    await host.RunAsync(default);

                    return 0;
                });

                app.Execute(args);

                Log.Information("Closing web host ({ApplicationContext})...", AppName);

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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = GetConfiguration(args);

            var host =  Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                      .CaptureStartupErrors(false)
                      .UseStartup<Startup>()
                      .UseContentRoot(AppContext.BaseDirectory)
                      .UseConfiguration(configuration)
                      .UseSerilog();
                });

            return host;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logFilePath = configuration["Serilog:FileLogPath"];

            var cfg = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, shared: true, rollingInterval: RollingInterval.Day)
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
