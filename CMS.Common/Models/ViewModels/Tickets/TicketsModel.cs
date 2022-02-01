using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketsModel
    {
        public int Idx { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserIdx { get; set; }
        public string Name { get; set; }
        public string TicketNumber { get; set; }
        public int AssigneeUserIdx { get; set; }
        public int? AssigneeUserGroupIdx { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int FirmIdx { get; set; }
        public int PriorityIdx { get; set; }
        public string PriorityName { get; set; }
        public int? TypeIdx { get; set; }
        public string TypeName { get; set; }
        public int Status { get; set; }
        public double? EstimatedTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public int? FirmUserIdx { get; set; }
        public string FirmName { get; set; }
        public string FirmUserName { get; set; }
        public double TimeAfterCreation { get; set; }
        public double TotalLogTimes { get; set; }
    }

    public class TicketListsModel
    {
        public List<TicketsModel> TicketsModels { get; set; }

        public TicketListsModel()
        {
            TicketsModels = new List<TicketsModel>();
        }
    }
}
