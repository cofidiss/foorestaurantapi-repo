using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace FooRestaurantApi.APILayer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                                 .AddJsonFile("appsettings.json")
                                 .Build();
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

            }
            finally
            {
                Log.CloseAndFlush();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .UseSerilog()
             .ConfigureAppConfiguration((hostingContext, config) =>
             {


                 config.AddEnvironmentVariables();
             })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
    }
}
