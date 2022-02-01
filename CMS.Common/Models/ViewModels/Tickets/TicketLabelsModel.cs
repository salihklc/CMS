using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketLabelsModel
    {
        public int Idx { get; set; }
        public int LabelIdx { get; set; }
        public int TicketIdx { get; set; }
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string Class { get; set; }
    }

    public class TicketLabelsChangeModel
    {
        public List<TicketLabelsModel> TicketLabels { get; set; }
        public int[] SelectedLabel { get; set; }
        public int TicketIdx { get; set; }
        public int UserIdx { get; set; }
    }
}
