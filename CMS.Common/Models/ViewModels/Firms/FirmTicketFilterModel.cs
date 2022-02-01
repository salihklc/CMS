using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Firms
{
    public class FirmTicketFilterModel
    {
        public int? FirmIdx { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? PriorityIdx { get; set; }
        public int? TypeIdx { get; set; }
        public string TicketNumber { get; set; }
    }
}
