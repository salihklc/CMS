using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketActivities
{
    public class TicketActivities : ViewComponent
    {
        private readonly ITicketService _ticketService;


        public TicketActivities(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {           
            return View(Idx);
        }
    }
}
