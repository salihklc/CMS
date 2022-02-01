using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketLogTimes : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
        public double LogTime { get; set; }
        public string Comment { get; set; }
        public int WorkingTypeIdx { get; set; }
        public WorkingTypes WorkingTypes { get; set; }
        public Tickets Tickets { get; set; }
        public User User { get; set; }
    }
}
