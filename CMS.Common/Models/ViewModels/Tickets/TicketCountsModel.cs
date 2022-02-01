using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketCountsModel
    {
        public int OpenedAllIssueCount { get; set; }
        public int ClosedIssueByMeCount { get; set; }
        public int CreatedIssueByMeCount { get; set; }
        public int OpenedIssueOnMeCount { get; set; }
        public int AllTicketsCount { get; set; }

        public TicketCountsModel()
        {
        }
      
    }
}
