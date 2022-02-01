using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class TicketAttachments : BaseEntity, IAggregateRoot
    {
        public int TicketIdx { get; set; }
        public int? CommentIdx { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentUrl { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
        public string AttachmentThumb { get; set; }
        public Tickets Tickets { get; set; }
        public TicketComments TicketComments { get; set; }
    }
}
