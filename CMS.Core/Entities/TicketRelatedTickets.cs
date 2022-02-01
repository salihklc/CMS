using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketRelatedTickets : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public Tickets Ticket { get; set; }
        public int RelatedTicketIdx { get; set; }
        public Tickets RelatedTicket { get; set; }
    }
}
