using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.TicketPeople
{
    public class TicketPeople : ViewComponent
    {
        private readonly ITicketService _ticketServices;
        public TicketPeople(ITicketService ticketService)
        {
            _ticketServices = ticketService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var model =await _ticketServices.GetTicketPeoples(Idx);
            return View(model);
        }
    }
}
