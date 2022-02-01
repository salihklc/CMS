using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Firms;

namespace CMS.WebSite.Controllers
{
    public class ReportController : Controller
    {
        private readonly IFirmService _firmService;
        public ReportController(IFirmService firmService)
        {
            _firmService = firmService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTicketReport()
        {
            return View();
        }

        public IActionResult TicketReport(FirmTicketFilterModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> _TicketReportCallBack(DTParameterModel dataTableRequest, int FirmIdx, string StartDate, string EndDate, int PriorityIdx, int TypeIdx, string TicketNumber)
        {
            FirmTicketFilterModel filter = new FirmTicketFilterModel
            {
                FirmIdx = FirmIdx,
                EndDate = EndDate,
                StartDate = StartDate,
                PriorityIdx = PriorityIdx,
                TicketNumber = TicketNumber,
                TypeIdx = TypeIdx
            };
            return Json(await _firmService.GetFirmTickets(dataTableRequest, filter));
        }

        public IActionResult CreteAlarmReport()
        {
            return View();
        }

        public IActionResult DynamicReports()
        {
            return View();
        }

         [HttpPost]
        public async Task<IActionResult> _DynamicReportCallBack(DTParameterModel dataTableRequest )
        {
            // FirmTicketFilterModel filter = new FirmTicketFilterModel
            // {
            //     FirmIdx = FirmIdx,
            //     EndDate = EndDate,
            //     StartDate = StartDate,
            //     PriorityIdx = PriorityIdx,
            //     TicketNumber = TicketNumber,
            //     TypeIdx = TypeIdx
            // };

            // return Json(await _firmService.GetFirmTickets(dataTableRequest, filter));
            return View();
        }
    }
}