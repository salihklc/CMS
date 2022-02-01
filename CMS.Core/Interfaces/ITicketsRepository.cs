using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface ITicketsRepository : IAsyncRepository<Tickets>
    {
        Task<IQueryable<Tickets>> GetAllMyTickets(int AssignerUserIdx);
        Task<DataTableResponse<Tickets>> GetAllMyTickets(int AssignerUserIdx, IDataTableRequest spec);
        Task<Tickets> GetTicketDetails(int Idx);
        Task<Tickets> GetByTicketNumberAsync(string TicketNumber);
        Task<IQueryable<TicketAttachments>> GetTicketAttachments(int TicketIdx);
        Task<IQueryable<TicketWatchers>> GetTicketWatchers(int TicketIdx);
        Task<IQueryable<TicketComments>> GetTicketComments(int TicketIdx);
        Task<IQueryable<TicketLogTimes>> GetTicketLogTimes(int TicketIdx);
        Task<IQueryable<TicketHistories>> GetTicketHistories(int TicketIdx);
        Task<DataTableResponse<Tickets>> GetAllMyDoneTickets(int AssignerUserIdx, IDataTableRequest spec);
        Task<DataTableResponse<Tickets>> GetAllOpenTickets(IDataTableRequest spec);
        Task<DataTableResponse<Tickets>> GetReportedMeTickets(int InsertUserIdx, IDataTableRequest spec);
        Task<DataTableResponse<Tickets>> GetAllTickets(IDataTableRequest spec);
        Task<DataTableResponse<TicketLabels>> GetTicketLabels(IDataTableRequest spec);
        Task<TicketWatchers> GetTicketWatchers(int userId, int ticketId);
        Task<TicketTypes> GetTicketType(int ticketId);
        Task<IQueryable<TicketRelatedTickets>> GetTicketRelateds(int TicketIdx);
        Task<IQueryable<Tickets>> GetTicketsByFirm(int FirmIdx);
        Task<MailTicketIntegrations> GetMailTicketsByThreadId(string ThreadId);
        Task<MailTicketIntegrations> GetMailTicketsByMessageId(string MessageId);
    }
}
