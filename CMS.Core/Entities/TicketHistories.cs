using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketHistories : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
        public int CurrentTicketStatus { get; set; }
        public int NewTicketStatus { get; set; }
        public int CurrentAssigneeUserIdx { get; set; }
        public int NewAssigneeUserIdx { get; set; }
        public string Description { get; set; }
        public string CaptureNow { get; set; }
        public int? MailSended { get; set; }

        public Tickets Tickets { get; set; }
        public User User { get; set; }
    }
}
