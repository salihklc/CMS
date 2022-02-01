using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace CMS.WebSite.Helpers
{

    public static class CookieExtensions
    {
        public static void SetCookie(this HttpContext httpContext, string key, string value, int ExpireTime)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(ExpireTime),
                IsEssential = true
            };

            httpContext.Response.Cookies.Append(key, value, options);

        }

        public static string GetCookie(this HttpContext httpContext, string key)
        {
            var value = httpContext.Request.Cookies[key];

            return value;
        }
    }
}