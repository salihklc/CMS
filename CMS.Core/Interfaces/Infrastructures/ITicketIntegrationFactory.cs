using CMS.Common.AppConstants;

namespace CMS.Core.Interfaces.Infrastructures
{
    public interface ITicketIntegrationFactory
    {
        IAbstractTicketIntegration GetIntegration();
        void SetIntegrationType(TypeOfTicketIntegration typeOfTicket);
    }
}