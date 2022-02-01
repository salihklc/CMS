

using System.Collections.Generic;
using CMS.Common.Models.CommonModels;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces.Infrastructures
{

    public interface IAbstractTicketIntegration
    {

        List<MailTicket> GetTicketById(ulong Id);
        void SendTicketReplyById(SendMailModel mailModel);

        WatchResponseModel WatchMail(WatchRequestModel requestModel);
    }

}