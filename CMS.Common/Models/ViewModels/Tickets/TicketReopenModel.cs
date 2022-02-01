using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketReopenModel
    {
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
        public string Description { get; set; }
        public int PriorityIdx { get; set; }
        public IFormFile[] Attachments { get; set; }
        public List<TicketAttachmentsModel> TicketAttachments { get; set; }
    }
}
