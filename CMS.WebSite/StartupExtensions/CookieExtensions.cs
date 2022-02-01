using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CMS.WebSite.Middleware;

namespace CMS.WebSite.StartupExtensions
{
    public static class CookieExtensions
    {
        // Some Extensions won't have both Services and Middleware.
        public static IServiceCollection ConfigureCustomCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
         .AddCookie(o =>
         {
             o.LoginPath = new PathString("/Account/Index");
             o.AccessDeniedPath = new PathString("/Account/Index");
             o.ExpireTimeSpan = TimeSpan.FromDays(5);
         });


            services.AddSession(options =>
            {
                options.Cookie.Name = ".CMS.Session";
                options.Cookie.IsEssential = true;
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCookiePolicy(this IApplicationBuilder app)
        {
            // For symmetry, you may wish to still put this in an extension,
            // but you could also decide not to.
            app.UseCookiePolicy();
            app.UseAuthentication();

            return app;
        }
    }
}