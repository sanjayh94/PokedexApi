using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace PokedexApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog Logging.
            // Serilog provides structured API event logs that can be easily extended to any supported log consuming platforms such as Splunk, Prometheus, Console, File, GrayLog, S3 and even Email!
            // https://github.com/serilog/serilog/wiki/Provided-Sinks
            // Setting up logging to Console for now

            #region SerilogConfiguration
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // Set logging level to warning and above from ASP.NET Core components as it can get chatty
            .Enrich.FromLogContext()
            .WriteTo.Console() // Writing logs to Console
            .CreateLogger();

            try
            {
                Log.Information("Application Starting up...");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            #endregion            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() //Set Serilog library for logging
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
