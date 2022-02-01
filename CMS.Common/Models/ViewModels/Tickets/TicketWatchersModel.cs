using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketWatchersModel
    {
        public int TicketIdx { get; set; }
        public string[] SelectedUsers { get; set; }
        public int UserIdx { get; set; }
        public int Status { get; set; }
    }
}
