using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common;
using CMS.Core.Interfaces;
using CMS.WebSite.Models;

namespace CMS.WebSite.Controllers
{
    public class HomeController : AuthController
    {
        private readonly IHomeService _homeService;
        public HomeController(IStringLocalizer<SharedResources> localizer,IHomeService homeService) : base(localizer)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _homeService.GetHomeCounters();
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
