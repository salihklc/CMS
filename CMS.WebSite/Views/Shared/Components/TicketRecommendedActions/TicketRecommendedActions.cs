using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketRecommendedActions
{
    public class TicketRecommendedActions : ViewComponent
    {
        private readonly ITicketService _ticketServices;
        public TicketRecommendedActions(ITicketService ticketService)
        {
            _ticketServices = ticketService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var model = await _ticketServices.GetTicketType(Idx);
            return View(model);
        }
    }
}
