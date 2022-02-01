
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using CMS.Common.AppConstants;
using CMS.Common.Models.CommonModels;

namespace CMS.Infrastructure.TicketIntegration.Integrators
{

    public class OutlookTicketIntegration : AbstractTicketIntegration
    {
        public OutlookTicketIntegration(IntegratorSettings integratorSettings, TypeOfTicketIntegration type) : base(type)
        {
        }

        public override List<MailTicket> GetTicketById(ulong Id)
        {
            throw new System.NotImplementedException();
        }

        public override void SendTicketReplyById(SendMailModel mailModel)
        {
            throw new System.NotImplementedException();
        }

        public override WatchResponseModel WatchMail(WatchRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }
    }

}