using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketStatusCategories : BaseEntity, IAggregateRoot
    {
        public string StatusCategoryName_TR { get; set; }
        public string StatusCategoryName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public ICollection<TicketStatus> TicketStatuses { get; set; }
    }
}
