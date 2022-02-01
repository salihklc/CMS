using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class WorkingTypes :BaseEntity, IAggregateRoot
    {

        public string WorkingTypeName_TR { get; set; }
        public string WorkingTypeName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string Color { get; set; }
        public ICollection<TicketLogTimes> TicketLogTimes { get; set; }
    }
}
