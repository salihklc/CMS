using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CMS.Storage;

namespace CMS.WebSite.StartupExtensions
{
    public static class DatabaseExtensions
    {

        public static IServiceCollection AddCustomDatabaseContext(this IServiceCollection service, IConfiguration Configuration, IHostingEnvironment env)
        {

            string logCnnString = Configuration.GetConnectionString("CmsLogDatabase");

            if (!env.IsDevelopment())
                logCnnString = logCnnString; //şifre çözme gibi işlem yapılabilir.
            
            service.AddDbContext<CmsLogDbContext>(options =>
                options.UseNpgsql(logCnnString), ServiceLifetime.Scoped);



            string cnnString = Configuration.GetConnectionString("CmsDatabase");

            if (!env.IsDevelopment())
                cnnString = cnnString; //şifre çözme gibi işlem yapılabilir.

            service.AddDbContext<CmsDbContext>(options =>
                options.UseNpgsql(cnnString), ServiceLifetime.Scoped);


            return service;
        }

    }
}