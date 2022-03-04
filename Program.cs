using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
                Assembly asm = Assembly.GetExecutingAssembly();

                var controlleractionlist = asm.GetTypes()
                        .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                        //  .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                        //  .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                        //  .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.BaseType, Fullname=x.ReturnType.FullName, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                        // .OrderBy(x => x.Controller).ToList();

                        .ToList();

                int counter = 0;
                foreach (var item in controlleractionlist)
                {
                    var name = item.Name.Replace("Controller","");
                    var splitName = SplitCamelCase(name);

                    //read
                    Console.WriteLine($"[Display(GroupName = \"{name}\", Name = \"Get\", Description = \"Can view {splitName.ToLower()}\")]");
                    Console.WriteLine($"{name}_Get = 0x{++counter},");

                    //write
                    Console.WriteLine($"[Display(GroupName = \"{name}\", Name = \"Post\", Description = \"Can create {splitName.ToLower()}\")]");
                    Console.WriteLine($"{name}_Post = 0x{++counter},");

                    //update
                    Console.WriteLine($"[Display(GroupName = \"{name}\", Name = \"Put\", Description = \"Can update {splitName.ToLower()}\")]");
                    Console.WriteLine($"{name}_Put = 0x{++counter},");

                    //delete
                    Console.WriteLine($"[Display(GroupName = \"{name}\", Name = \"Delete\", Description = \"Can delete {splitName.ToLower()}\")]");
                    Console.WriteLine($"{name}_Delete = 0x{++counter},\n");

                }

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

        static string SplitCamelCase(string source)
        {
            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
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
                    webBuilder.UseStartup<Startup>();       //To run locally

                    //webBuilder.UseStartup<Startup>()        //When pushing
                    //.UseKestrel(options =>
                    //{
                    //    options.ListenAnyIP(Int32.Parse(System.Environment.GetEnvironmentVariable("PORT") ?? "5050"));
                    //});
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
