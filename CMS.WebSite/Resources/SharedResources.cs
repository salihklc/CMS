

using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CMS.WebSite
{
    public class SharedResources
    {
    }

    public static class Cultures
    {
        private static Dictionary<string, string> CultureInfos;

        static Cultures()
        {
            CultureInfos = new Dictionary<string, string>()
            {
              {"Türkçe","tr-TR"},
              {"English", "en-US"}
            };
        }

        public static Dictionary<string, string> GetCultureInfos()
        {
            return CultureInfos;
        }
    }
}