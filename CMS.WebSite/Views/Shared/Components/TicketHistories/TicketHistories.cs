using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketHistories
{
    public class TicketHistories : ViewComponent
    {
        private readonly ITicketService _ticketService;
        public TicketHistories(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int TicketIdx)
        {
            var model = await _ticketService.GetTicketHistories(TicketIdx);
            return View(model);
        }
    }
}
