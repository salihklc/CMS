using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketRelatedIssues
{
    public class TicketRelatedIssues : ViewComponent
    {
        private readonly ITicketService _ticketServices;
        public TicketRelatedIssues(ITicketService ticketService)
        {
            _ticketServices = ticketService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var model = await _ticketServices.GetTicketRelateds(Idx);
            return View(model);
        }
    }
}
