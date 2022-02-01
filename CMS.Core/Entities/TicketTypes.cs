using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketTypes : BaseEntity, IAggregateRoot
    {
        public string TicketTypeName_TR { get; set; }
        public string TicketTypeName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Descrption_EN { get; set; }
        public string RecommendedActions { get; set; }
        public double EstimatedTime { get; set; }
        public ICollection<Tickets> Tickets { get; set; }
    }
}
