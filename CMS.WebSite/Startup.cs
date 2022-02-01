using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CMS.WebSite.StartupExtensions;

namespace CMS.WebSite
{
    public class Startup
    {

        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            services.ConfigureCustomCookiePolicy();

            services.AddCustomDependencies();
            
            services.AddCustomConfiguration(Configuration, CurrentEnvironment);

            services.AddCustomDatabaseContext(Configuration, CurrentEnvironment);

            services.AddCustomisedMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomEnv(env);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSession();

            //app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseCustomCookiePolicy();

            app.UseCustomisedMvc();
        }
    }
}
