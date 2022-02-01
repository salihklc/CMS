using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketRelatedIssues
    {
        public int Idx { get; set; }
        public int TicketIdx { get; set; }
        public int RelatedTicketIdx { get; set; }
        public TicketsModel RelatedTicket { get; set; }
        public Person AssigneeUser { get; set; }
        public TicketStatusModel TicketStatus { get; set; }
        public int UserIdx { get; set; }
    }
}
