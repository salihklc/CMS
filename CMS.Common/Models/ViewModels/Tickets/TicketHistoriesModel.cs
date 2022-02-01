using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketHistoriesModel
    {
        public int Idx { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
        public UserModel User { get; set; }
        public int CurrentTicketStatusIdx { get; set; }
        public TicketStatusModel CurrentTicketStatusModel { get; set; }
        public int NewTicketStatusIdx { get; set; }
        public TicketStatusModel NewTicketStatusModel { get; set; }
        public int CurrentAssignneUserIdx { get; set; }
        public UserModel CurrentAssigneeUser { get; set; }
        public int NewAssigneeUserIdx { get; set; }
        public UserModel NewAssigneeUser { get; set; }
        public string Description { get; set; }
        public string CaptureNow { get; set; }
        public int? MailSended { get; set; }

    }
}
