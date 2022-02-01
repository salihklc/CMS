using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CMS.Common;
using CMS.Common.Interfaces;
using CMS.Core.Entities;
using CMS.Core.Interfaces;
using CMS.WebSite.Helpers;
using CMS.WebSite.Models;

namespace CMS.WebSite.Controllers
{
    public class LogController : AuthController
    {
        ILogsRepository _logRepository;
        private readonly ILogger _logger;

        public LogController(IStringLocalizer<SharedResources> localizer,
            ILogsRepository logsRepository, ILogger<LogController> logger) : base(localizer)
        {
            _logRepository = logsRepository;
            _logger = logger;
        }

        public IActionResult Logs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _LogsCallback(DTParameterModel dataTableRequest)
        {
            _logger.LogDebug("Datatable request test : {req}", JsonConvert.SerializeObject(dataTableRequest));

            return Json(await _logRepository.ListAsync(dataTableRequest));
        }

        public async Task<IActionResult> ExportExcel()
        {
            return Content("");
        }
    }
}
