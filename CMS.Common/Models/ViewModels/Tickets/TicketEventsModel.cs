using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketEventsModel
    {
        public int Idx { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public int TicketIdx { get; set; }
        /// <summary>
        /// Comment : 1 , StatusChange : 2, ChangeAssignee : 3
        /// </summary>
        public int EventTypeIdx { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// EventType'a göre join olup mail atacaz
        /// </summary>
        public int ContentIdx { get; set; }
        public int MailisSended { get; set; }
    }
}
