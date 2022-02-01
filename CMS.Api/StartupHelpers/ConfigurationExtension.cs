using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CMS.Common.Models.CommonModels;
using CMS.Storage;

namespace CMS.Api.StartupHelpers
{
    public static class ConfigurationExtension
    {

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection service, IConfiguration Configuration, IHostingEnvironment env)
        {

            var integratorConfig = Configuration.GetSection("IntegratorSettings");
            service.Configure<IntegratorSettings>(integratorConfig);

            var folderConfig = Configuration.GetSection("Folders");
            service.Configure<FoldersSettings>(folderConfig);

            return service;
        }

    }
}