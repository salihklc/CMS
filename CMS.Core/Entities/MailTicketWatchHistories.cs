using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class MailTicketWatchHistory : BaseEntity, IAggregateRoot
    {
        public string Email { get; set; }
        public ulong HistoryId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string IntegrationName { get; set; }
        public int Stopped { get; set; }

    }
}