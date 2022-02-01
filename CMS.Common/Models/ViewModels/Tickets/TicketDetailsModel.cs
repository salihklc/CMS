using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketDetailsModel
    {
        public int TicketIdx { get; set; }
        public TicketTypesModel TicketTypes { get; set; }
        public PrioritiesModel Priorities { get; set; }
        public TicketStatusModel TicketStatus { get; set; }
        public List<TicketLabelsModel> TicketLabels { get; set; }
        public TicketHeaderModel TicketHeader { get; set; }
    }
}
