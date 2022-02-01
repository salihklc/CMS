using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using CMS.Common;
using CMS.Common.Models.CommonModels;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Middleware
{
    public class UserProfileRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            string culture = "tr-TR";

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                var defaultLocalizationValue = httpContext.Features.Get<IRequestCultureFeature>();

                ProviderCultureResult defaultLocal;

                if (defaultLocalizationValue != null &&
                string.IsNullOrEmpty(defaultLocalizationValue.RequestCulture.Culture.Name))
                {
                    defaultLocal = new ProviderCultureResult(defaultLocalizationValue.RequestCulture.Culture.Name);
                }
                else
                {
                    var allowedCulteres = Cultures.GetCultureInfos().First();

                    var region = new RegionInfo(allowedCulteres.Value);

                    defaultLocal = new ProviderCultureResult(region.Name);
                }

                return Task.FromResult(defaultLocal);
            }

            var cultureFromCookie = httpContext.GetCookie(StaticModels.CookieLanguageKey);
            
            
            if (!string.IsNullOrWhiteSpace(cultureFromCookie))
            {
                culture = cultureFromCookie;
            }

            var requestCulture = new ProviderCultureResult(culture);

            return Task.FromResult(requestCulture);

        }
    }


}