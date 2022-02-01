using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketChangeInfoModel
    {
        public int Idx { get; set; }
        public int TypeIdx { get; set; }
        public int PriorityIdx { get; set; }
        public int FirmIdx { get; set; }
        public int FirmUserIdx { get; set; }
        public int EnvironmentIdx { get; set; }
        public int UserIdx { get; set; }
        public double? EstimatedTime { get; set; }
    }
}
