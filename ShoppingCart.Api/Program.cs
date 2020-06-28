using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ShoppingCart.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.Print(msg));

            try
            {
                var app = typeof(Program).Assembly.GetName();
                Log.Information("{Application} version {Version} starting up", app.FullName, app.Version);
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5001");
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var env = hostingContext.HostingEnvironment;

                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                            .AddJsonFile("appsettings.serilog.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                        if (env.IsDevelopment())
                        {
                            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                            if (appAssembly != null)
                            {
                                config.AddUserSecrets(appAssembly, optional: true);
                            }
                        }
                        config.AddEnvironmentVariables();

                        if (args != null)
                        {
                            config.AddCommandLine(args);
                        }
                    });
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("EnvironmentName", hostingContext.HostingEnvironment.EnvironmentName);
                    });
                });
    }
}
