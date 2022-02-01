using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Core.Entities;
using CMS.Core.Interfaces;
using CMS.Core.Interfaces.Infrastructures;
using CMS.Core.Mapping.Infrastructure;
using CMS.Core.Services;
using CMS.Infrastructure.TicketIntegration;
using CMS.Storage;
using CMS.Storage.Repositories;
using CMS.WebSite.Middleware;

namespace CMS.WebSite.StartupExtensions
{
    public static class DependencyExtensions
    {

        public static IServiceCollection AddCustomDependencies(this IServiceCollection service)
        {

            //AutoMapper
            service.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            service.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            service.AddScoped<DbContext, CmsLogDbContext>();
            service.AddScoped<DbContext, CmsDbContext>();


            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ICityRepository, CityRepository>();
            service.AddScoped<IRolesRepository, RoleRepository>();
            service.AddScoped<IPermissionsRepository, PermissionsRepository>();
            service.AddScoped<ITicketsRepository, TicketRepository>();
            service.AddScoped<IFirmsRepository, FirmsRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<ILogsRepository, LogsRepository>();


            service.AddScoped<ITicketService, TicketServices>();
            service.AddScoped<IGeneralServices, GeneralServices>();
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IPermissionService, PermissionService>();
            service.AddScoped<IDefinitionService, DefinitionService>();
            service.AddScoped<IFirmService, FirmService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IHomeService, HomeService>();
            service.AddScoped<IUserService, UserService>(x => new UserService(
               x.GetRequiredService<IUserRepository>(), x.GetRequiredService<IMapper>(),
               x.GetRequiredService<IAsyncRepository<UserRoles>>(),
               x.GetRequiredService<IFirmsRepository>(),
               x.GetRequiredService<IOptions<FoldersSettings>>(),
               x.GetRequiredService<ILogger<UserService>>()));


            service.AddScoped<ITicketIntegrationFactory, TicketIntegrationFactory>();


            service.AddLogging(loggingBuilder =>
                     loggingBuilder.AddSerilog(dispose: true));

            //Register the Permission policy handlers
            service.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            service.AddScoped<IAuthorizationHandler, PermissionAuthorizeHandler>(x => new PermissionAuthorizeHandler(x.GetRequiredService<IUserService>()));


            return service;
        }

    }
}