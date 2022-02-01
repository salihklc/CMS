using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketStatus : BaseEntity, IAggregateRoot
    {
        public string StatusName_TR { get; set; }
        public string StatusName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public int CategoryIdx { get; set; }
        public TicketStatusCategories TicketStatusCategories { get; set; }
        public ICollection<Tickets> Tickets { get; set; }
    }
}
