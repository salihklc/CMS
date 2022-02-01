using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CMS.WebSite.StartupExtensions
{
    public static class EnvBuilderExtensions
    {
        // // Some Extensions won't have both Services and Middleware.
        // public static IConfiguration ConfigureEnv(this IConfiguration configuration, IHostingEnvironment env)
        // {
        //      IConfigurationBuilder builder = null;
        //      IConfiguration Configuration;

        //     if (env.EnvironmentName == "Development")
        //     {
        //         builder = new ConfigurationBuilder()
        //         .SetBasePath(env.ContentRootPath)
        //         .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
        //     }

        //     if (env.EnvironmentName == "Production")
        //     {
        //         builder = new ConfigurationBuilder()
        //         .SetBasePath(env.ContentRootPath)
        //         .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
        //     }


        //     if (env.EnvironmentName == "Test")
        //     {
        //         builder = new ConfigurationBuilder()
        //         .SetBasePath(env.ContentRootPath)
        //         .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
        //     }

        //     if (builder != null)
        //     {
        //         Configuration = builder.Build();
        //     }
        //     else
        //         Configuration = configuration;


        //     return Configuration;
        // }


        public static IApplicationBuilder UseCustomEnv(this IApplicationBuilder application, IHostingEnvironment env){
            
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                application.UseHsts();
            }

            return application;
        }
    }
}