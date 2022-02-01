using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Labels : BaseEntity, IAggregateRoot
    {
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string Class { get; set; }
        public ICollection<TicketLabels> TicketLabels { get; set; }
    }
}
