using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketCommentsModel
    {
        public int Idx { get; set;}
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; } 
        public DateTime? UpdateDate { get; set; }
        public string Comment { get; set; }
        public UserModel InsertedUser { get; set; }  
        public List<TicketAttachmentsModel> TicketAttachments { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public int TicketIdx { get; set; }
        public bool SendMailtoCustomer { get; set; }
        public TicketCommentsModel()
        {
            TicketAttachments = new List<TicketAttachmentsModel>();
        }
      
    }
}
