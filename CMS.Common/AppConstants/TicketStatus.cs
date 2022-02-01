using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.AppConstants
{
    public enum ITicketStatus
    {
        OPEN=1,
        WAITINGFORSUPPORT,    
        WAITINGFOR3RDPARTY,
        WAITINGFORVALIDATION,
        WAITINGFORMANAGER,
        REOPENED,
        DRAFT,
        SUPPORTWORKING,
        INPROGRESS,
        WAITINGFORCUSTOMER,
        DEVELOPING,
        USERTESTING,
        WAITINGFOREQUIPMENT,
        ANALYSIS,
        CONTROLLING,
        PENDING,
        PLANNINGANDSCHEDULING,
        REVIEW,
        CLOSED,
        RESOLVED,
        DONE,
        REJECTED

    }

    public enum ITicketStatusCategory
    {
        TODO=1,
        INPROGRESS,
        DONE,
        NOCATEGORY
    }

    public enum GeneralStatus
    {
        Active,
        Passsive
    }

   
}
