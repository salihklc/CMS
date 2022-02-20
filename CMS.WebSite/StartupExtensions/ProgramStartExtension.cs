using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace CMS.WebSite.StartupExtensions
{
    public static class ProgramStartExtension
    {
        public static void BuilWebHost(string[] args)
        {
            var pathToContentRoot = Directory.GetCurrentDirectory();
            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            IConfigurationBuilder builder = null;
            IConfiguration Configuration;

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            builder = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (envName == "Development")
            {
                builder = builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }

            if (envName == "Production")
            {
                builder = builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
            }

            if (envName == "Test")
            {
                builder = builder.AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
            }


            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.WithProperty("InsertUserIdx",0)
            .Enrich.WithProperty("Status", 0)
            .CreateLogger();

            Log.Information("Program is starting...");

            var host = WebHost.CreateDefaultBuilder(webHostArgs)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            host.Run();

        }

    }
}