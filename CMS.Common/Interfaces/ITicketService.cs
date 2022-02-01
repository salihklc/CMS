using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Tickets;

namespace CMS.Common.Interfaces
{
    public interface ITicketService
    {
        Task<TicketListsModel> GetAllTickets();
        Task<List<TicketsModel>> AllTickets();
        Task<DataTableResponse<TicketsModel>> GetAllMyActiveTickets(int AssiggneeUserIdx, IDataTableRequest dataTableRequest);
        Task<DataTableResponse<TicketsModel>> GetAllMyDoneTickets(int AssigneeeUserIdx, IDataTableRequest request);
        Task<DataTableResponse<TicketsModel>> GetAllOpenTickets(IDataTableRequest request);
        Task<DataTableResponse<TicketsModel>> GetAllTickets(IDataTableRequest request);
        Task<DataTableResponse<TicketsModel>> GetReportedMeTickets(int InsertUserIdx, IDataTableRequest request);
        Task<int> SyncTicketFromGmail(GmailPushDataModel dataModel);
        Task<WatchResponseModel> WatchMail(WatchRequestModel watchRequest);
        Task<int> CreateTicket(CreateTicketModel createTicketModel);
        Task<TicketDetailsModel> GetTicketDetails(int Idx);
        Task<List<TicketAttachmentsModel>> GetTicketAttachments(int TicketIdx);
        Task<TicketPeoplesModel> GetTicketPeoples(int Idx);
        Task<TicketsModel> GetTickets(int Idx);
        Task<List<TicketCommentsModel>> GetTicketComments(int Idx);
        Task<List<TicketLogTimesModel>> GetTicketLogTimes(int Idx);
        Task<int> AddTicketComment(TicketCommentsModel comment);
        Task<int> AddTicketLogTimes(TicketLogTimesModel ticketLogTimesModel);
        Task<int> ChangeTicketAssiggnee(TicketAssigneeModel ticketAssiggnee);
        Task<int> ChangeTicketState(TicketChangeStateModel changeStateModel);
        Task<int> ChangeTicketDescription(TicketDescriptionModel ticketDescriptionModel);
        Task<List<TicketHistoriesModel>> GetTicketHistories(int Idx);
        Task<int> ReopenTicket(TicketReopenModel ticketReopenModel);
        Task<TicketLogTimesModel> GetTicketLogTime(int Idx);
        Task<List<TicketLabelsModel>> GetTicketLabels();
        Task<DataTableResponse<TicketLabelsModel>> GetTicketLabels(IDataTableRequest request);
        Task<int> AddTicketLabels(TicketLabelsChangeModel ticketLabelsModel);
        Task<int> ChangeTicketInfos(TicketChangeInfoModel ticketChangeInfoModel);
        Task<int> AddTicketWatchers(TicketWatchersModel ticketWatchersModel);
        Task<int> AddTicketEvents(TicketEventsModel ticketEventsModel);
        Task<TicketTypesModel> GetTicketType(int TicketIdx);
        Task<List<TicketRelatedIssues>> GetTicketRelateds(int TicketIdx);
        Task<List<TicketsModel>> GetTicketsByFirm(int FirmIdx);
        Task<int> AddTicketRelateds(TicketRelatedIssues ticketRelatedModel);
        Task<int> DeleteTicketRelateds(int Idx);
        Task<int> SendTicketReply(SendMailModel model);
        Task<Attachment> DownloadAttachment(int key);
        Task<TicketCountsModel> GetAllTicketsCounts(int AssiggneeUserIdx);
        Task<bool> DeleteAttachment(int Idx);
        Task<int> SaveAttachments(List<IFormFile> Attachments, int TicketIdx, int UserIdx, int? CommentIdx = null);

    }
}
