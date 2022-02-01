using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketWorkItems :  BaseEntity, IAggregateRoot
    {
        public string WorkName { get; set; }
        public string Description { get; set; }
        public int WorkOrder { get; set; }
        public int AssigneeUserIdx { get; set; }
        public bool IsCompleted { get; set; }
        public int TicketIdx { get; set; }
        public Tickets Tickets { get; set; }
    }
}
