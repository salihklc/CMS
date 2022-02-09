using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using Serilog;
using Microsoft.EntityFrameworkCore;
using CMS.TaskManager.Business.Database;
using CMS.TaskManager.Business.Logic.Models;
using System.Reflection;
using System.Collections.Generic;

namespace CMS.TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string startLogFile = "StartLog.txt";


            var pathToContentRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string startLogFilePath = Path.Combine(pathToContentRoot, startLogFile);


            var webHostArgs = args.Where(arg => arg != "--console").ToArray();
            
            List<string> msg = new List<string>() { $"Application Startings on path {pathToContentRoot}" };

            File.AppendAllLines(startLogFilePath, msg, System.Text.Encoding.UTF8);

            IConfigurationBuilder builder = null;
            IConfiguration Configuration;
            

            var envName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            
            msg.Add($"Environment is {envName}");
            
            File.AppendAllLines(startLogFilePath, msg, System.Text.Encoding.UTF8);

            if (string.IsNullOrEmpty(envName))
            {
                envName = "Production";
            }

            if (envName == "Development")
            {
                builder = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }

            if (envName == "Production")
            {
                builder = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
            }

            if (envName == "Test")
            {
                builder = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
            }


            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();
            Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .ReadFrom.Configuration(Configuration)
             .Enrich.WithProperty("Application","CmsTaskManager")
             .Enrich.WithProperty("InsertUserIdx", 0)
             .Enrich.WithProperty("Status", 0)
             .CreateLogger();


            msg.Add("Configuration is build");
            File.AppendAllLines(startLogFilePath, msg, System.Text.Encoding.UTF8);

            Log.Information("Program is starting...");

            var cnnString = Configuration.GetConnectionString("CmsDatabase");

            var host = Host.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)
                .UseWindowsService()
                 .ConfigureAppConfiguration((context, config) =>
                 {
                     // configure the app here.
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    var cnnSettings = Configuration.GetSection("ConnectionStrings");
                    services.Configure<ConnectionStrings>(cnnSettings);
                    
                    var applicationSettings = Configuration.GetSection("ApplicationConfig");
                    services.Configure<ApplicationConfig>(applicationSettings);
                    
                    services.AddDbContext<CmsDbContext>(options => options.UseNpgsql(cnnString), ServiceLifetime.Transient);

                    services.AddHostedService<Worker>();
                })
                .UseSerilog()
                .Build();

            host.Run();


            //CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            services.AddHostedService<Worker>();
        //        });
    }
}
