using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketComments
{
    public class TicketComments : ViewComponent
    {
        private readonly ITicketService _ticketService;


        public TicketComments(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {

            var comments = await _ticketService.GetTicketComments(Idx);
            return View(comments);
        }
    }
}
