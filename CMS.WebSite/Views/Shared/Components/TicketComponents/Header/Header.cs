using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketComponents.Header
{
    public class Header : ViewComponent
    {
        private readonly ITicketService _ticketService;

        public Header(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var model = _ticketService.GetTicketDetails(Idx);
            return View(model);
        }
    }
}
