using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketWatchers : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }

        public Tickets Tickets { get; set; }
        public User Users { get; set; }
    }
}
