using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using CMS.Common.Models.ViewModels.Configurations;
using CMS.WebSite.Helpers;
using CMS.Common.Models;
using CMS.Common.Models.CommonModels;
using CMS.Common;
using Microsoft.AspNetCore.Localization;
using System;
using Microsoft.AspNetCore.Http;

namespace CMS.WebSite.Controllers
{
    public class ConfigurationController : BaseController
    {
        public ConfigurationController(IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
        }


        public async Task<IActionResult> GetLanguage()
        {
            var allowedCultures = Cultures.GetCultureInfos();

            var selecItems = new List<SelectListItem>();

            var defaultLanguage = HttpContext.GetCookie(StaticModels.CookieLanguageKey);

            foreach (var item in allowedCultures)
            {
                var selected = item.Value == defaultLanguage;
                selecItems.Add(new SelectListItem() { Value = item.Value, Text = item.Key, Selected = selected });
            }

            return Json(selecItems);
        }

        [HttpPost]
        public async Task<IActionResult> SetLanguage(LocalizationModel model)
        {
            HttpContext.SetCookie(StaticModels.CookieLanguageKey, model.LocalizationCode, 999999999);

   //         Response.Cookies.Append(
   //    CookieRequestCultureProvider.DefaultCookieName,
   //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(model.LocalizationCode)),
   //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
   //);

            return Json(new JsonResponse{ IsSuccess = true, Message = _sharedLocalizer["LanguageHasChanged"].Value });
        }
    }
}