using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketLabels : BaseEntity , IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int LabelIdx { get; set; }
        public Tickets Ticket { get; set; }
        public Labels Label { get; set; }
    }
}
