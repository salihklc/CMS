using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketHeaderModel
    {
        public TicketsModel TicketHeader { get; set; }
        public List<ButtonPermissions> Permissions { get; set; }
    }

    public class ButtonPermissions
    {
        public string PermissionName { get; set; }
        public int PermissionNo { get; set; }
        public bool AuthorizeUser { get; set; }
    }
}
