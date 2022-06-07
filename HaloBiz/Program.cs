using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.Auths.PermissionParts;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore;
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
        public static async Task Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)       
                .CreateLogger();

            try
            {
                // CreatePermissions();
                (await BuildWebHostAsync(args)).Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }   
        }

        private static async Task<IWebHost> BuildWebHostAsync(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

            await webHost.Services.AddAdminRole();
            return webHost;
        }

        private static void CreatePermissions()
        {
            int counter = 0;// Enum.GetValues(typeof(Permissions)).GetUpperBound(0);


            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                    .ToList();

            List<int> controllerIdList = new List<int>();
            foreach (var item in controlleractionlist)
            {

                var name = item.Name.Replace("Controller", "");

                var splitName = SplitCamelCase(name);
                var module = (ModuleName)item.GetCustomAttributes<ModuleName>().FirstOrDefault();
                if (module == null)
                    throw new Exception($"{name} controller does not have module specified");

                var (moduleName, controllerId) = module.GetModuleAndControllerId();
                if (controllerIdList.Contains(controllerId))
                {
                    throw new Exception($"Duplicate controller ID: {controllerId} in {item.Name}");
                }

                controllerIdList.Add(controllerId);

                var isFourAdded = Enum.TryParse(typeof(Permissions), string.Concat(name, "_get"), true, out var permission)
                                && Enum.TryParse(typeof(Permissions), string.Concat(name, "_post"), true, out var permission2)
                                && Enum.TryParse(typeof(Permissions), string.Concat(name, "_put"), true, out var permission3)
                                && Enum.TryParse(typeof(Permissions), string.Concat(name, "_delete"), true, out var permission4);

                //check that this controller with its 4 permissions do not exist
                if (isFourAdded) continue;  //no problem

                counter = (controllerId * 10) + 1000;

                //read
                Console.WriteLine($"[Display(ShortName=\"{moduleName}\",GroupName = \"{name}\", Name = \"Get\", Description = \"Can view {splitName.ToLower()}\")]");
                Console.WriteLine($"{name}_Get = 0x{++counter},");

                //write
                Console.WriteLine($"[Display(ShortName=\"{moduleName}\",GroupName = \"{name}\", Name = \"Post\", Description = \"Can create {splitName.ToLower()}\")]");
                Console.WriteLine($"{name}_Post = 0x{++counter},");

                //update
                Console.WriteLine($"[Display(ShortName=\"{moduleName}\",GroupName = \"{name}\", Name = \"Put\", Description = \"Can update {splitName.ToLower()}\")]");
                Console.WriteLine($"{name}_Put = 0x{++counter},");

                //delete
                Console.WriteLine($"[Display(ShortName=\"{moduleName}\",GroupName = \"{name}\", Name = \"Delete\", Description = \"Can delete {splitName.ToLower()}\")]");
                Console.WriteLine($"{name}_Delete = 0x{++counter},\n");
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
