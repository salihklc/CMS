using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketLogTimes
{
    public class TicketLogTimes : ViewComponent
    {
        private readonly ITicketService _ticketService;


        public TicketLogTimes(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {


            var ticketLogTimes = await _ticketService.GetTicketLogTimes(Idx);
            return View(ticketLogTimes);
        }
    }
}
