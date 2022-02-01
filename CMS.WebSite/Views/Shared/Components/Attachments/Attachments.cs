using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Tickets;

namespace CMS.WebSite.Views.Shared.Components.Attachments
{
    public class Attachments : ViewComponent
    {
        private readonly ITicketService _ticketService;


        public Attachments(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var ticketAttachments = await _ticketService.GetTicketAttachments(Idx);
            AttachmentListModel model = new AttachmentListModel();
            model.TicketAttachmentsModels = ticketAttachments;
            model.TicketIdx = Idx;
            return View(model);
        }

    }
}