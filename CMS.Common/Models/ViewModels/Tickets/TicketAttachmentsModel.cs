using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketAttachmentsModel
    {
        public int Idx { get; set; }
        public int TicketIdx { get; set; }
        public int? CommentIdx { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentUrl { get; set; }
        public string FileType { get; set; }
        
        public int? FileSize { get; set; }
        //Base64 str
        public string AttachmentThumb { get; set; }
    }

    public class AttachmentListModel
    {
        public int TicketIdx { get; set; }
        public List<TicketAttachmentsModel> TicketAttachmentsModels { get; set; }
    }
}
