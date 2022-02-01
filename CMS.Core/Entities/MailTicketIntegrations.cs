using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class MailTicketIntegrations : BaseEntity, IAggregateRoot
    {
        public string MessageId { get; set; }
        public string ThreadId { get; set; }
        public ulong HistoryId { get; set; }
        public string ShortMessage { get; set; }
        public string MessageBodyText { get; set; }
        public string MessageBodyHtml { get; set; }

        public string Subject { get; set; }
        public string From { get; set; } // mail ve adı
        public string ErrorsTo { get; set; } // sadece maili
        public string Delivered { get; set; } // adı ve maili
        public string DeliveredTo { get; set; } //Mail adresi

        public DateTime SendDate { get; set; }
        public DateTime ReceiveDate { get; set; }

        public string Headers { get; set; }

        public int IntegrationType { get; set; }
        public string IntegrationName { get; set; }

        public int? TicketId { get; set; }
        public int? CommentId { get; set; }
    }
}
