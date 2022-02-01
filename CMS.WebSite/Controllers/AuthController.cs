using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common;
using CMS.WebSite.Models;

namespace CMS.WebSite.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {
        public AuthController(IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
        }
    }
}
