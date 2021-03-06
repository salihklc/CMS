using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketComments : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
        public string Comment { get; set; }
        public int? ParentCommentIdx { get; set; }
        public Tickets Tickets { get; set; }
        public User User { get; set; }
        public ICollection<TicketAttachments> TicketAttachments { get; set; }
    }
}
