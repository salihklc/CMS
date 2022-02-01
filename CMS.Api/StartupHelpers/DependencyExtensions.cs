using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using CMS.Api.Filters;
using CMS.Common.Interfaces;
using CMS.Core.Entities;
using CMS.Core.Interfaces;
using CMS.Core.Interfaces.Infrastructures;
using CMS.Core.Mapping.Infrastructure;
using CMS.Core.Services;
using CMS.Infrastructure.TicketIntegration;
using CMS.Storage.Repositories;

namespace CMS.Api.StartupHelpers
{
    public static class DependencyExtensions
    {

        public static IServiceCollection AddCustomDependencies(this IServiceCollection service)
        {

            //AutoMapper
            service.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ICityRepository, CityRepository>();
            service.AddScoped<IRolesRepository, RoleRepository>();
            service.AddScoped<IPermissionsRepository, PermissionsRepository>();
            service.AddScoped<ITicketsRepository, TicketRepository>();

            service.AddScoped<ITicketService, TicketServices>();
            service.AddScoped<IGeneralServices, GeneralServices>();
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IPermissionService, PermissionService>();

            // service.AddScoped<IUserService, UserService>(x => new UserService(
            //     x.GetRequiredService<IUserRepository>(),x.GetRequiredService<IMapper>(),x.GetRequiredService<IAsyncRepository<UserRoles>>()));

            service.AddLogging(loggingBuilder =>
                     loggingBuilder.AddSerilog(dispose: true));

            service.AddScoped<ITicketIntegrationFactory, TicketIntegrationFactory>();

            service.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });


            return service;
        }

    }
}