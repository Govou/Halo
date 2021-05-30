using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace HaloBiz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)       
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }   
        }

        private static IConfiguration GetConfiguration(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serilog.json", optional: true, reloadOnChange: true);

            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    var configurationRoot = configApp.Build();

                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);

                    configApp.AddEnvironmentVariables();
                    configApp.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();       //To run locally

                    webBuilder.UseStartup<Startup>()        //When pushing
                    .UseKestrel(options =>
                    {
                        options.ListenAnyIP(Int32.Parse(System.Environment.GetEnvironmentVariable("PORT") ?? "5050"));
                    });
                })
                .UseSerilog((hostContext, loggerConfig) =>
                {
                    var configuration = hostContext.Configuration;

                    var connectionString = string.Empty;

                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        connectionString = configuration.GetConnectionString("DefaultConnection");
                    }
                    else
                    {
                        var server = configuration["DbServer"];
                        var port = configuration["DbPort"];
                        var user = configuration["DbUser"];
                        var password = configuration["DbPassword"];
                        var database = configuration["Database"];

                        connectionString = $"Server={server},{port};Database={database};User Id={user};Password={password};";
                    }
                    
                    loggerConfig
                        .ReadFrom.Configuration(configuration)
                        .WriteTo.MSSqlServer(connectionString,
                            sinkOptions: new MSSqlServerSinkOptions { TableName = "HalobizLogs" }, null, null,
                            LogEventLevel.Information, null, null, null, null)
                        .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
                });
    }
}
