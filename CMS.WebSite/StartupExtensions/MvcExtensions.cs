using System.Globalization;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using CMS.WebSite.Middleware;
using CMS.Common.Models.ViewModels.Users;
using CMS.WebSite.Resources;
using CMS.WebSite.Filters;

namespace CMS.WebSite.StartupExtensions
{
    public static class MvcExtensions
    {
        static RequestLocalizationOptions localizationOptions;

        public static IServiceCollection AddCustomisedMvc(this IServiceCollection services)
        {
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMemoryCache();
            services.AddTransient<SharedViewLocalizer>();

            // Uygulamamızın desteklediği dilleri tanımlıyoruz.
            localizationOptions = new RequestLocalizationOptions();

            var cultures = Cultures.GetCultureInfos();

            string defaultCulture = "tr-TR";

            cultures.TryGetValue("Türkçe", out defaultCulture);

            localizationOptions.DefaultRequestCulture = new RequestCulture(defaultCulture);

            foreach (var item in cultures)
            {
                localizationOptions.SupportedCultures.Add(new CultureInfo(item.Value));
                localizationOptions.SupportedUICultures.Add(new CultureInfo(item.Value));
            }

            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new UserProfileRequestCultureProvider());

            services.AddLocalization(option => { option.ResourcesPath = "Resources"; });

            services
                .AddMvc(options =>
                    {
                        options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                        options.Filters.Add(typeof(ViewBagFilter));
                    }
                ).ConfigureApiBehaviorOptions(options => { options.SuppressInferBindingSourcesForParameters = true; })
                .AddViewLocalization()
                //.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization(option =>
                {
                    option.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
                })
                .AddJsonOptions(options =>
                {
                    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Always;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserModel>())
                .AddSessionStateTempDataProvider();

            return services;
        }

        public static IApplicationBuilder UseCustomisedMvc(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(localizationOptions);
            app.UseMvc(mvcRoutes =>
            {
                mvcRoutes.MapRoute(
                    name: "default",
                    template: "/{controller=Account}/{action=Index}/{id?}");
            });

            return app;
        }
    }
}