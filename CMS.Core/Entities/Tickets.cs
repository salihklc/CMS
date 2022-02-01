using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Tickets : BaseEntity,IAggregateRoot
    {
        public string Name { get; set; }
        public string TicketNumber { get; set; }
        public int? TypeIdx { get; set; }
        public int AssigneeUserIdx { get; set; }
        public int? AssigneeUserGroupIdx { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int FirmIdx { get; set; }
        public int? FirmUserIdx { get; set; }
        public int? FirmProjectIdx { get; set; }
        public double? EstimatedTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int PriorityIdx { get; set; }
        public int? EnvironmentIdx { get; set; }
        public string DocumentLinks { get; set; }      
        public string Email { get; set; }       
        public int? ParentTicketIdx { get; set; }
        public ICollection<TicketAttachments> TicketAttachments { get; set; }
        public ICollection<TicketComments> TicketComments { get; set; }
        public ICollection<TicketHistories> TicketHistories { get; set; }
        public ICollection<TicketLogTimes> TicketLogTimes { get; set; }
        public ICollection<TicketRelatedTickets> TicketRelatedTickets { get; set; }
        public ICollection<TicketWatchers> TicketWatchers { get; set; }
        public ICollection<TicketWorkItems> TicketWorkItems { get; set; }
        public ICollection<TicketEvents> TicketEvents { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public Priorities Priorities { get; set; }
        public ICollection<TicketLabels> TicketLabels { get; set; }
        public TicketTypes TicketTypes { get; set; }
        public Firms Firms { get; set; }

        [JsonIgnore]
        public User FirmUser { get; set; }
    }
}
