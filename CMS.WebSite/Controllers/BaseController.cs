using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common;

namespace CMS.WebSite.Controllers
{
    public class BaseController : Controller
    {
        protected IStringLocalizer<SharedResources> _sharedLocalizer;
        public BaseController(IStringLocalizer<SharedResources> localizer)
        {
            _sharedLocalizer = localizer;
        }

    }
}
