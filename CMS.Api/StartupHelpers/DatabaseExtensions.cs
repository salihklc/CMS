using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CMS.Storage;

namespace CMS.Api.StartupHelpers
{
    public static class DatabaseExtensions
    {

        public static IServiceCollection AddCustomDatabaseContext(this IServiceCollection service,IConfiguration Configuration,IHostingEnvironment env)
        {

            string cnnString = Configuration.GetConnectionString("CmsDatabase");

            if (!env.IsDevelopment())
                cnnString = cnnString; //şifre çözme gibi işlem yapılabilir.
            service.AddScoped<DbContext, CmsDbContext>();

            service.AddDbContext<CmsDbContext>(options =>
                options.UseSqlServer(cnnString), ServiceLifetime.Scoped);

            return service;
        }

    }
}