using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class TicketRepository : EfRepository<Tickets>, ITicketsRepository
    {
        public TicketRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<Tickets>> GetAllMyTickets(int AssignerUserIdx)
        {
            return (_dbContext as CmsDbContext).Tickets.Include(r => r.TicketStatus).ThenInclude(k => k.TicketStatusCategories).Include(l => l.Firms).Where(r => r.AssigneeUserIdx == AssignerUserIdx);
        }

        public async Task<DataTableResponse<Tickets>> GetAllMyTickets(int AssignerUserIdx, IDataTableRequest spec)
        {
            return DataTableEvaluator<Tickets>.GetQueryable((_dbContext as CmsDbContext).Tickets.Include(k => k.Firms).Include(l => l.FirmUser).Where(r => r.AssigneeUserIdx == AssignerUserIdx && r.TicketStatus.CategoryIdx != (int)ITicketStatusCategory.DONE), spec);
        }

        public async Task<DataTableResponse<Tickets>> GetAllMyDoneTickets(int AssignerUserIdx, IDataTableRequest spec)
        {
            return DataTableEvaluator<Tickets>.GetQueryable((_dbContext as CmsDbContext).Tickets.Include(k => k.Firms).Include(l => l.FirmUser).Where(r => r.AssigneeUserIdx == AssignerUserIdx && r.TicketStatus.CategoryIdx == (int)ITicketStatusCategory.DONE), spec);
        }

        public async Task<DataTableResponse<Tickets>> GetAllOpenTickets(IDataTableRequest spec)
        {
            return DataTableEvaluator<Tickets>.GetQueryable((_dbContext as CmsDbContext).Tickets.Include(k => k.Firms).Include(l => l.FirmUser).Where(r => r.TicketStatus.CategoryIdx != (int)ITicketStatusCategory.DONE), spec);
        }

        public async Task<DataTableResponse<Tickets>> GetReportedMeTickets(int InsertUserIdx, IDataTableRequest spec)
        {
            return DataTableEvaluator<Tickets>.GetQueryable((_dbContext as CmsDbContext).Tickets.Include(k => k.Firms).Include(l => l.FirmUser).Where(r => r.InsertUserIdx == InsertUserIdx), spec);
        }
        public async Task<DataTableResponse<Tickets>> GetAllTickets(IDataTableRequest spec)
        {
            return DataTableEvaluator<Tickets>.GetQueryable((_dbContext as CmsDbContext).Tickets.Include(k => k.Firms).Include(l => l.FirmUser), spec);
        }
        public async Task<Tickets> GetTicketDetails(int Idx)
        {
            return await (_dbContext as CmsDbContext).Tickets.Include(r => r.Priorities).Include(k => k.TicketStatus)
                .ThenInclude(l => l.TicketStatusCategories).Include(x => x.TicketTypes).Include(m => m.TicketLabels).ThenInclude(n => n.Label).FirstOrDefaultAsync(r => r.Idx == Idx);
        }

        public async Task<IQueryable<TicketAttachments>> GetTicketAttachments(int TicketIdx)
        {
            return (_dbContext as CmsDbContext).TicketAttachments.Where(r => r.TicketIdx == TicketIdx && r.Status == (int)GeneralStatus.Active);
        }

        public async Task<IQueryable<TicketWatchers>> GetTicketWatchers(int TicketIdx)
        {
            return (_dbContext as CmsDbContext).TicketWatchers.Include(k => k.Users).Where(r => r.TicketIdx == TicketIdx);
        }

        public async Task<IQueryable<TicketComments>> GetTicketComments(int TicketIdx)
        {
            return (_dbContext as CmsDbContext).TicketComments.Include(r => r.User).Include(t => t.Tickets).Include(a => a.TicketAttachments).Where(c => c.TicketIdx == TicketIdx);
        }

        public async Task<IQueryable<TicketLogTimes>> GetTicketLogTimes(int TicketIdx)
        {
            return (_dbContext as CmsDbContext).TicketLogTimes.Include(r => r.WorkingTypes).Include(u => u.User).Where(c => c.TicketIdx == TicketIdx);
        }
        public async Task<IQueryable<TicketHistories>> GetTicketHistories(int TicketIdx)
        {
            return (_dbContext as CmsDbContext).TicketHistories.Include(r => r.Tickets).Include(u => u.User).Where(c => c.TicketIdx == TicketIdx);
        }

        public async Task<DataTableResponse<TicketLabels>> GetTicketLabels(IDataTableRequest spec)
        {
            return DataTableEvaluator<TicketLabels>.GetQueryable((_dbContext as CmsDbContext).TicketLabels, spec);
        }
        public async Task<Tickets> GetByTicketNumberAsync(string TicketNumber)
        {
            return (_dbContext as CmsDbContext).Tickets.FirstOrDefault(f => f.TicketNumber == TicketNumber);
        }
        public async Task<TicketWatchers> GetTicketWatchers(int userId, int ticketId)
        {
            return await (_dbContext as CmsDbContext).TicketWatchers.FirstOrDefaultAsync(f => f.UserIdx == userId && f.TicketIdx == ticketId);
        }

        public async Task<TicketTypes> GetTicketType(int ticketId)
        {
            var ticket = await (_dbContext as CmsDbContext).Tickets.Include(r => r.TicketTypes).FirstOrDefaultAsync(r => r.Idx == ticketId);
            return ticket.TicketTypes;
        }

        public async Task<IQueryable<TicketRelatedTickets>> GetTicketRelateds(int TicketIdx)
        {
            var relatedTickets = (_dbContext as CmsDbContext).TicketRelatedTickets.Include(r => r.RelatedTicket).ThenInclude(k => k.TicketStatus).Where(k => k.TicketIdx == TicketIdx);
            return relatedTickets;
        }

        public async Task<IQueryable<Tickets>> GetTicketsByFirm(int FirmIdx)
        {
            var tickets = (_dbContext as CmsDbContext).Tickets.Where(k => k.FirmIdx == FirmIdx);
            return tickets;
        }

        public async Task<MailTicketIntegrations> GetMailTicketsByThreadId(string ThreadId)
        {
            var mailTicketIntegrations = await (_dbContext as CmsDbContext).MailTicketIntegrations.Where(k => k.ThreadId == ThreadId).FirstOrDefaultAsync();
            return mailTicketIntegrations;
        }

        public async Task<MailTicketIntegrations> GetMailTicketsByMessageId(string MessageId)
        {
            var mailTicketIntegrations = await (_dbContext as CmsDbContext).MailTicketIntegrations.Where(k => k.MessageId == MessageId).FirstOrDefaultAsync();
            return mailTicketIntegrations;
        }
    }
}
