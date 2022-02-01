

using System;
using System.Security.Claims;

namespace CMS.WebSite.Helpers
{
    public static class PrincipalExtensions
    {
        public static string GetCulture(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue("localizationapp:culture");
        }

        public static string GetUICulture(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue("localizationapp:uiculture");
        }
    }
}