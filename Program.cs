using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HaloBiz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();       //To run locally

                    //webBuilder.UseStartup<Startup>()        //When pushing
                    //.UseKestrel(options =>
                    //{
                    //    options.ListenAnyIP(Int32.Parse(System.Environment.GetEnvironmentVariable("PORT") ?? "5050"));
                    //});
                });
    }
}
