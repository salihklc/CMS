using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CMS.Common.AppConstants;
using CMS.Common.Models.CommonModels;
using CMS.Core.Interfaces.Infrastructures;
using CMS.Infrastructure.TicketIntegration.Integrators;

namespace CMS.Infrastructure.TicketIntegration
{

    public class TicketIntegrationFactory : ITicketIntegrationFactory
    {
        private AbstractTicketIntegration Integration;
        private readonly ILogger<GmailTicketIntegration> _logger;
        private TypeOfTicketIntegration _typeOfTicket = TypeOfTicketIntegration.Google;

        private IntegratorSettings integratorSettings;

        public TicketIntegrationFactory(IOptions<IntegratorSettings> settings, ILogger<GmailTicketIntegration> logger)
        {
            integratorSettings = settings.Value as IntegratorSettings;
            _logger = logger;
        }

        public IAbstractTicketIntegration GetIntegration()
        {
            switch (_typeOfTicket)
            {
                case TypeOfTicketIntegration.Google:
                    Integration = new GmailTicketIntegration(integratorSettings, _typeOfTicket,_logger);
                    break;
                case TypeOfTicketIntegration.Outlook:
                    Integration = new OutlookTicketIntegration(integratorSettings,_typeOfTicket);
                    break;
                default:
                    Integration = new GmailTicketIntegration(integratorSettings,_typeOfTicket,_logger);
                    break;
            }

            return Integration;
        }

        public void SetIntegrationType(TypeOfTicketIntegration typeOfTicket)
        {
            _typeOfTicket = typeOfTicket;
        }
    }
}