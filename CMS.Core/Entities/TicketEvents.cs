using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketEvents : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public Tickets Tickets { get; set; }
        public int EventTypeIdx { get; set; }
        public int ContentIdx { get; set; }
        public int MailisSended { get; set; }      
    }
}
