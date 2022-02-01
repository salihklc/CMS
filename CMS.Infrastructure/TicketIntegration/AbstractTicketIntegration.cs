using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using CMS.Common.AppConstants;
using CMS.Common.Models.CommonModels;
using CMS.Core.Entities;
using CMS.Core.Interfaces.Infrastructures;

namespace CMS.Infrastructure.TicketIntegration
{

    public abstract class AbstractTicketIntegration : IAbstractTicketIntegration
    {
        public readonly string IntegrationName;
        public readonly TypeOfTicketIntegration IntegrationType;
        public AbstractTicketIntegration(TypeOfTicketIntegration type)
        {
            IntegrationName = Enum.GetName(typeof(TypeOfTicketIntegration), type);
            IntegrationType = type;

        }

        public abstract List<MailTicket> GetTicketById(ulong Id);

        public abstract void SendTicketReplyById(SendMailModel mailModel);

        public abstract WatchResponseModel WatchMail(WatchRequestModel requestModel);
    }
}