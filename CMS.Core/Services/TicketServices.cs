using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.Core.Interfaces;
using System.Linq;
using CMS.Common.AppConstants;
using CMS.Common.Models.CommonModels;
using CMS.Core.Entities;
using CMS.Common.Models.ViewModels.Users;
using CMS.Core.Interfaces.Infrastructures;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CMS.Common.Helpers;

namespace CMS.Core.Services
{
    public class TicketServices : ITicketService
    {
        private readonly ILogger<TicketServices> _logger;
        private readonly ITicketsRepository _ticketRepository;
        private readonly IAsyncRepository<TicketAttachments> _ticketAttachmentRepository;
        private readonly IAsyncRepository<MailTicketIntegrations> _mailTicketIntegrationRepository;
        private readonly IAsyncRepository<MailTicketWatchHistory> _mailTicketWatchHistory;
        private readonly ITicketIntegrationFactory _ticketIntegrationFactory;
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAsyncRepository<TicketComments> _ticketCommentRepository;
        private readonly IAsyncRepository<TicketLogTimes> _ticketLogTimesRepository;
        private readonly IAsyncRepository<TicketHistories> _ticketHistoriesRepository;
        private readonly IAsyncRepository<TicketStatus> _ticketStatusRepository;
        private readonly IAsyncRepository<Labels> _labelRepository;
        private readonly IAsyncRepository<TicketLabels> _ticketLabelRepository;
        private readonly IAsyncRepository<TicketWatchers> _ticketWatchersRepository;
        private readonly IAsyncRepository<TicketEvents> _ticketEventsRepository;
        private readonly IAsyncRepository<TicketTypes> _ticketTypesRepository;
        private readonly IAsyncRepository<TicketRelatedTickets> _ticketRelatedsRepository;
        private readonly IAsyncRepository<Firms> _firmRepository;
        private readonly IAsyncRepository<Priorities> _priorityRepository;
        private readonly IOptions<FoldersSettings> _FolderOptions;

        public TicketServices(ILogger<TicketServices> logger, ITicketsRepository ticketsRepository, ITicketIntegrationFactory ticketIntegrationFactory,
            IUserRepository userRepository, IHostingEnvironment hostingEnvironment, IAsyncRepository<TicketAttachments> ticketAttachmentRepository,
            IAsyncRepository<TicketComments> ticketCommentRepository, IAsyncRepository<TicketLogTimes> ticketLogTimesRepository,
            IAsyncRepository<TicketHistories> ticketHistoriesRepository, IAsyncRepository<TicketStatus> ticketStatusRepository,
            IAsyncRepository<Labels> labelRepository, IAsyncRepository<TicketLabels> ticketLabelRepository,
            IAsyncRepository<MailTicketIntegrations> mailTicketIntegrationRepository, IAsyncRepository<TicketWatchers> ticketWatchersRepository,
            IAsyncRepository<TicketEvents> ticketEventsRepository, IAsyncRepository<TicketTypes> ticketTypesRepository,
            IAsyncRepository<MailTicketWatchHistory> mailTicketWatchHistory, IAsyncRepository<TicketRelatedTickets> ticketRelatedsRepository,
            IAsyncRepository<Firms> firmRepository, IAsyncRepository<Priorities> priorityRepository,
            IOptions<FoldersSettings> folderOptions)
        {
            _logger = logger;

            _ticketRepository = ticketsRepository;
            _userRepository = userRepository;
            _ticketIntegrationFactory = ticketIntegrationFactory;
            _hostingEnvironment = hostingEnvironment;
            _ticketAttachmentRepository = ticketAttachmentRepository;
            _ticketCommentRepository = ticketCommentRepository;
            _ticketLogTimesRepository = ticketLogTimesRepository;
            _ticketHistoriesRepository = ticketHistoriesRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _labelRepository = labelRepository;
            _ticketLabelRepository = ticketLabelRepository;
            _mailTicketIntegrationRepository = mailTicketIntegrationRepository;
            _ticketWatchersRepository = ticketWatchersRepository;
            _ticketEventsRepository = ticketEventsRepository;
            _ticketTypesRepository = ticketTypesRepository;
            _mailTicketWatchHistory = mailTicketWatchHistory;
            _ticketRelatedsRepository = ticketRelatedsRepository;
            _FolderOptions = folderOptions;
            _firmRepository = firmRepository;
            _priorityRepository = priorityRepository;
        }

        public async Task<int> CreateTicket(CreateTicketModel createTicketModel)
        {
            try
            {
                var ticket = createTicketModel.xCopyTo<Tickets>();
                ticket.Status = (int)ITicketStatus.OPEN;
                ticket.AssigneeUserGroupIdx = 1; // Default user group
                ticket.TicketNumber = DateTime.Now.Ticks.ToString();
                if (createTicketModel.TypeIdx.GetValueOrDefault() > 0)
                {
                    ticket.EstimatedTime = (await _ticketTypesRepository.GetByIdAsync(createTicketModel.TypeIdx.GetValueOrDefault())).EstimatedTime;
                }

                var res = await _ticketRepository.AddAsync(ticket);
                if (createTicketModel.Attachments != null && createTicketModel.Attachments.Count() > 0)
                {
                    await SaveAttachments(createTicketModel.Attachments, ticket.Idx, createTicketModel.InsertUserIdx);
                }
                return res.Idx;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<int> SaveAttachments(List<IFormFile> Attachments, int TicketIdx, int UserIdx, int? CommentIdx = null)
        {
            string filePath = _FolderOptions.Value.Attachments;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (IFormFile file in Attachments)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string nfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string fullPath = Path.Combine(filePath, nfileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    string base64FileImage = "";
                    if (Path.GetExtension(fileName) == ".png" || Path.GetExtension(fileName) == ".jpeg" || Path.GetExtension(fileName) == ".jpg")
                    {
                        base64FileImage = ImageProcessHelper.CreateBase64ThumbFromImage(fullPath, 70, 60);
                    }
                    

                    TicketAttachments ticketAttachments = new TicketAttachments();
                    ticketAttachments.AttachmentName = Guid.NewGuid().ToString();
                    ticketAttachments.AttachmentUrl = Path.Combine(filePath, nfileName);
                    ticketAttachments.TicketIdx = TicketIdx;
                    ticketAttachments.InsertDate = DateTime.Now;
                    ticketAttachments.InsertUserIdx = UserIdx;
                    ticketAttachments.FileType = Path.GetExtension(fileName);
                    ticketAttachments.CommentIdx = CommentIdx;
                    ticketAttachments.AttachmentThumb = base64FileImage;
                    var attachment = await _ticketAttachmentRepository.AddAsync(ticketAttachments);
                }
            }

            return 1;
        }

        public async Task<Attachment> DownloadAttachment(int key)
        {
            Attachment result = new Attachment();

            var attchmentData = await _ticketAttachmentRepository.GetByIdAsync(key);

            result.FileName = attchmentData.AttachmentName + attchmentData.FileType;

            using (FileStream SourceStream = File.Open(attchmentData.AttachmentUrl, FileMode.Open))
            {
                result.ByteData = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result.ByteData, 0, (int)SourceStream.Length);
            }

            return result;
        }

        public async Task<int> SaveAttachments(List<Attachment> Attachments, int TicketIdx, int UserIdx, int? CommentIdx = null)
        {
            string filePath = _FolderOptions.Value.Attachments;


            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var file in Attachments)
            {
                if (file.ByteData.Length > 0 && file.Size > 0)
                {

                    string fileName = file.FileName;
                    string nfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string fullPath = Path.Combine(filePath, nfileName);

                    File.WriteAllBytes(fullPath, file.ByteData);

                    string base64FileImage = null;

                    try
                    {
                        base64FileImage = ImageProcessHelper.CreateBase64ThumbFromImage(fullPath, 70, 60);
                    }
                    catch (Exception)
                    {

                    }

                    TicketAttachments ticketAttachments = new TicketAttachments();
                    ticketAttachments.AttachmentName = Guid.NewGuid().ToString();
                    ticketAttachments.AttachmentUrl = Path.Combine(filePath, nfileName);
                    ticketAttachments.AttachmentThumb = base64FileImage;
                    ticketAttachments.TicketIdx = TicketIdx;
                    ticketAttachments.InsertDate = DateTime.Now;
                    ticketAttachments.InsertUserIdx = UserIdx;
                    ticketAttachments.FileType = Path.GetExtension(fileName);
                    ticketAttachments.CommentIdx = CommentIdx;
                    var attachment = await _ticketAttachmentRepository.AddAsync(ticketAttachments);
                }
            }

            return 1;
        }

        public async Task<int> SyncTicketFromGmail(GmailPushDataModel dataModel)
        {
            int returnCode = 200;

            try
            {

                _ticketIntegrationFactory.SetIntegrationType(TypeOfTicketIntegration.Google);

                var integration = _ticketIntegrationFactory.GetIntegration();

                #region last watch datasından itibaren sync edicez sonra sync'i gelen push'un history id ile update edicez.

                DTParameterModel lastWatch = new DTParameterModel()
                {
                    GetAll = false,
                    Draw = 1,
                    Start = 0,
                    Columns = new List<DTColumn>(){
                        new DTColumn(){Data = "Idx",Name="Idx" }
                    },
                    Length = 1,
                    Order = new List<DTOrder>() {
                        new DTOrder(){Column = 0,Dir="desc"}
                    }
                };

                var lastWatchData = (await _mailTicketWatchHistory.ListAsync(lastWatch)).Data.FirstOrDefault();

                var ticketsDatas = integration.GetTicketById(lastWatchData.HistoryId);

                //Gelen data create or comment olarak eklenecek. Push data yeni ya da olağana cevap olabilir.
                if (ticketsDatas != null)
                {

                    foreach (var ticketData in ticketsDatas)
                    {
                        _logger.LogInformation("ticket Data {data}", JsonConvert.SerializeObject(ticketData));

                        //Bu ilk kez açılırken kullanılacak unique Id daha sonra aynı ThreadId ile gelen kayıtlar comment olarak eklenecek.
                        var existTicket = await _ticketRepository.GetByTicketNumberAsync(ticketData.ThreadId);

                        int? commentIdx = null;
                        int ticketIdx = 0;

                        if (existTicket != null && existTicket.Idx > 0)
                        {
                            ticketData.From = ticketData.From.Substring(ticketData.From.IndexOf('<') + 1, (ticketData.From.LastIndexOf('>') - ticketData.From.IndexOf('<') - 1));

                            var user = await _userRepository.GetUserByEmail(ticketData.From);
                            TicketComments comments = new TicketComments()

                            {
                                Comment = ticketData.MessageBodyText,
                                TicketIdx = existTicket.Idx,
                                InsertDate = DateTime.Now,
                                InsertUserIdx = user == null ? -1 : user.Idx,
                                UpdateUserIdx = user == null ? -1 : user.Idx,
                                UserIdx = user == null ? -1 : user.Idx
                            };

                            var comment = await _ticketCommentRepository.AddAsync(comments);
                            commentIdx = comment.Idx;

                            ticketIdx = existTicket.Idx;
                        }
                        else
                        {
                            var allFirms = await _firmRepository.ListAllAsync();

                            var mailFirm = allFirms.Where(f => (f.Email == ticketData.From || (f.IsDefault ?? false))).FirstOrDefault();

                            var allPriorities = await _priorityRepository.ListAllAsync();

                            var defaultPriority = allPriorities.FirstOrDefault();


                            Tickets tickets = new Tickets()
                            {
                                Status = (int)ITicketStatus.OPEN,
                                TicketNumber = ticketData.ThreadId,
                                Name = ticketData.Subject,
                                Description = ticketData.MessageBodyText,
                                Summary = ticketData.ShortMessage,
                                AssigneeUserGroupIdx = 1,
                                Email = ticketData.From,
                                PriorityIdx = defaultPriority.Idx,
                                StartDate = ticketData.ReceiveDate,
                                FirmIdx = mailFirm.Idx
                            };

                            var newTicket = await _ticketRepository.AddAsync(tickets);

                            ticketIdx = newTicket.Idx;
                        }


                        if (ticketData.Attechments != null && ticketData.Attechments.Count() > 0)
                        {
                            await SaveAttachments(ticketData.Attechments, ticketIdx, -1, commentIdx);
                        }

                        _logger.LogInformation("Integration map starts: {data}", JsonConvert.SerializeObject(ticketData));

                        int sendDateIsValid = DateTime.Compare(ticketData.SendDate, new DateTime(1970, 1, 1, 1, 1, 1, 1));
                        int receiveDateIsValid = DateTime.Compare(ticketData.ReceiveDate, new DateTime(1970, 1, 1, 1, 1, 1, 1));

                        if (sendDateIsValid <= 0)
                        {
                            ticketData.SendDate = new DateTime(1970, 1, 1, 1, 1, 1, 1);
                        }

                        if (receiveDateIsValid <= 0)
                        {
                            ticketData.ReceiveDate = new DateTime(1970, 1, 1, 1, 1, 1, 1);
                        }

                        MailTicketIntegrations integrationData = new MailTicketIntegrations()
                        {
                            CommentId = commentIdx,
                            Delivered = ticketData.Delivered,
                            HistoryId = ticketData.HistoryId,
                            DeliveredTo = ticketData.DeliveredTo,
                            ErrorsTo = ticketData.ErrorsTo,
                            From = ticketData.From,
                            Headers = ticketData.Headers,
                            InsertDate = DateTime.Now,
                            InsertUserIdx = -1,
                            IntegrationName = ticketData.IntegrationName,
                            IntegrationType = (int)ticketData.IntegrationType,
                            MessageBodyHtml = ticketData.MessageBodyHtml,
                            MessageBodyText = ticketData.MessageBodyText,
                            MessageId = ticketData.MessageId,
                            ReceiveDate = ticketData.ReceiveDate,
                            SendDate = ticketData.SendDate,
                            ShortMessage = ticketData.ShortMessage,
                            Status = 0,
                            Subject = ticketData.Subject,
                            ThreadId = ticketData.ThreadId,
                            TicketId = ticketIdx,
                            UpdateDate = DateTime.Now,
                            UpdateUserIdx = -1
                        };

                        _logger.LogInformation("Integration maps ends {integrationData}", integrationData);

                        integrationData.TicketId = ticketIdx;
                        integrationData.CommentId = commentIdx;

                        var mailData = await _mailTicketIntegrationRepository.AddAsync(integrationData);
                    }

                }

                //Gelen push ile update ettik.
                lastWatchData.HistoryId = dataModel.HistoryId;
                lastWatchData.Email = dataModel.Email;
                lastWatchData.UpdateDate = DateTime.Now;
                lastWatchData.UpdateUserIdx = -2;

                _logger.LogInformation("watch data update edilecek {data}",
                 JsonConvert.SerializeObject(lastWatchData));

                await _mailTicketWatchHistory.UpdateAsync(lastWatchData);

                #endregion


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Expcetion message: {message}", ex.Message);

                return 400;
            }

            return returnCode;
        }

        public async Task<WatchResponseModel> WatchMail(WatchRequestModel requestModel)
        {
            _ticketIntegrationFactory.SetIntegrationType(TypeOfTicketIntegration.Google);

            var integration = _ticketIntegrationFactory.GetIntegration();

            var watchMail = integration.WatchMail(requestModel);

            if (requestModel.IsWatchRequest)
            {
                _logger.LogInformation("watch kaydı geliyor. {watchMail}", JsonConvert.SerializeObject(watchMail));
                MailTicketWatchHistory modelToSave = new MailTicketWatchHistory()
                {
                    ExpireDate = watchMail.ExpireDate,
                    HistoryId = watchMail.HistoryId,
                    Email = "",
                    UpdateDate = DateTime.Now,
                    InsertDate = DateTime.Now,
                    UpdateUserIdx = -1,
                    InsertUserIdx = -1,
                    IntegrationName = ""
                };

                try
                {
                    _logger.LogInformation("DATA TO SAVE {data}", JsonConvert.SerializeObject(modelToSave));

                    var result = await _mailTicketWatchHistory.AddAsync(modelToSave);
                    _logger.LogInformation("saved data {data}", JsonConvert.SerializeObject(result));

                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "data {f}", ex.Message);
                }
            }
            else
            {
                DTParameterModel lastWatch = new DTParameterModel()
                {
                    GetAll = false,
                    Draw = 1,
                    Start = 0,
                    Length = 1,
                    Order = new List<DTOrder>() {
                        new DTOrder(){Column = 0,Dir="desc"}
                    },
                    Columns = new List<DTColumn>() { new DTColumn() { Data = "Idx", Name = "Idx" } }
                };

                var lastWatchData = (await _mailTicketWatchHistory.ListAsync(lastWatch)).Data.FirstOrDefault();

                lastWatchData.UpdateDate = DateTime.Now;
                lastWatchData.Stopped = 1;

                await _mailTicketWatchHistory.UpdateAsync(lastWatchData);

            }

            /*
            test data call
            */
            DTParameterModel lastWatch1 = new DTParameterModel()
            {
                GetAll = false,
                Draw = 1,
                Start = 0,
                Length = 1,
                Order = new List<DTOrder>() {
                        new DTOrder(){Column = 0,Dir="desc"}
                },
                Columns = new List<DTColumn>() { new DTColumn() { Data = "Idx", Name = "Idx" } }
            };

            var lastWatchData1 = (await _mailTicketWatchHistory.ListAsync(lastWatch1)).Data.FirstOrDefault();

            _logger.LogInformation("Last Watch History Id: {lastWatchData}", lastWatchData1);

            return watchMail;
        }

        public async Task<TicketListsModel> GetAllMyActiveTickets(int AssiggneeUserIdx)
        {
            var tickets = await _ticketRepository.GetAllMyTickets(AssiggneeUserIdx);

            TicketListsModel ticketListsModel = new TicketListsModel();

            foreach (var ticket in tickets.Where(r => r.TicketStatus.TicketStatusCategories.Idx != (int)ITicketStatusCategory.DONE))
            {
                ticketListsModel.TicketsModels.Add(ticket.xCopyTo<TicketsModel>());
            }

            return ticketListsModel;
        }

        private async Task<DataTableResponse<TicketsModel>> SetDatatable(DataTableResponse<Tickets> ticketEntity)
        {
            DataTableResponse<TicketsModel> returnModel = new DataTableResponse<TicketsModel>();
            returnModel.Draw = ticketEntity.Draw;
            returnModel.RecordsFiltered = ticketEntity.RecordsFiltered;
            returnModel.RecordsTotal = ticketEntity.RecordsTotal;

            returnModel.Data = ticketEntity.Data.Select(f =>
            new TicketsModel()
            {
                Idx = f.Idx,
                Summary = f.Summary,
                Description = f.Description,
                InsertDate = f.InsertDate,
                TicketNumber = f.TicketNumber,
                Name = f.Name,
                FirmName = f.Firms == null ? "" : f.Firms.FirmName,
                FirmUserName = f.FirmUser == null ? "" : f.FirmUser.FirstName + " " + f.FirmUser.LastName,
                EstimatedTime = f.EstimatedTime,
                TimeAfterCreation = (DateTime.Now - f.InsertDate).TotalMinutes,
                Status = f.Status
            }).ToList();

            return returnModel;
        }

        public async Task<DataTableResponse<TicketsModel>> GetAllMyActiveTickets(int AssigneeeUserIdx, IDataTableRequest request)
        {
            var ticketEntity = await _ticketRepository.GetAllMyTickets(AssigneeeUserIdx, request);
            return await SetDatatable(ticketEntity);
        }
        public async Task<DataTableResponse<TicketsModel>> GetAllMyDoneTickets(int AssigneeeUserIdx, IDataTableRequest request)
        {
            var ticketEntity = await _ticketRepository.GetAllMyDoneTickets(AssigneeeUserIdx, request);
            return await SetDatatable(ticketEntity);
        }

        public async Task<DataTableResponse<TicketsModel>> GetAllOpenTickets(IDataTableRequest request)
        {
            var ticketEntity = await _ticketRepository.GetAllOpenTickets(request);
            return await SetDatatable(ticketEntity);
        }
        public async Task<DataTableResponse<TicketsModel>> GetReportedMeTickets(int InsertUserIdx, IDataTableRequest request)
        {
            var ticketEntity = await _ticketRepository.GetReportedMeTickets(InsertUserIdx, request);
            return await SetDatatable(ticketEntity);
        }

        public async Task<DataTableResponse<TicketsModel>> GetAllTickets(IDataTableRequest request)
        {
            var ticketEntity = await _ticketRepository.GetAllTickets(request);
            return await SetDatatable(ticketEntity);
        }
        public async Task<TicketListsModel> GetAllTickets()
        {
            var Tickets = await _ticketRepository.ListAllAsync();

            TicketListsModel ticketListsModel = new TicketListsModel();

            foreach (var ticket in Tickets)
            {
                ticketListsModel.TicketsModels.Add(
                    ticket.xCopyTo<TicketsModel>());
            }

            return ticketListsModel;
        }

        public async Task<List<TicketsModel>> AllTickets()
        {
            var Tickets = await _ticketRepository.ListAllAsync();

            List<TicketsModel> ticketListsModel = new List<TicketsModel>();

            foreach (var ticket in Tickets)
            {
                ticketListsModel.Add(
                    ticket.xCopyTo<TicketsModel>());
            }

            return ticketListsModel;
        }

        public async Task<TicketDetailsModel> GetTicketDetails(int Idx)
        {
            var TicketDetails = await _ticketRepository.GetTicketDetails(Idx);
            var Labels = TicketDetails.TicketLabels != null ? TicketDetails.TicketLabels.Select(k => k.Label).ToList() : new List<Labels>();
            TicketDetailsModel ticketDetails = new TicketDetailsModel
            {
                Priorities = TicketDetails.Priorities.xCopyTo<PrioritiesModel>(),

                TicketStatus = TicketDetails.TicketStatus.xCopyTo<TicketStatusModel>(),

                TicketHeader = new TicketHeaderModel
                {
                    TicketHeader = TicketDetails.xCopyTo<TicketsModel>()
                },
                TicketIdx = TicketDetails.Idx
            };
            if (TicketDetails.TicketTypes != null)
            {
                ticketDetails.TicketTypes = TicketDetails.TicketTypes.xCopyTo<TicketTypesModel>();
            }
            else
            {
                ticketDetails.TicketTypes = new TicketTypesModel();
            }
            ticketDetails.TicketLabels = new List<TicketLabelsModel>();
            foreach (var label in Labels)
            {
                ticketDetails.TicketLabels.Add(label.xCopyTo<TicketLabelsModel>());
            }
            return ticketDetails;
        }

        //public Task<TicketDetailsModel> GetTicketDetails(int Idx)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<TicketPeoplesModel> GetTicketPeoples(int Idx)
        {
            TicketPeoplesModel model = new TicketPeoplesModel();
            var ticket = await _ticketRepository.GetByIdAsync(Idx);
            var assigneeUser = new User();
            if (ticket.AssigneeUserIdx > 0)
            {
                assigneeUser = await _userRepository.GetByIdAsync(ticket.AssigneeUserIdx);
            }
            //var userGroup = await userGroupRepository.GetByIdAsync(ticket.AssigneeUserGroupIdx ?? 1);

            var watchers = await _ticketRepository.GetTicketWatchers(Idx);

            var createdUser = await _userRepository.GetByIdAsync(ticket.InsertUserIdx);

            model.Assignee = assigneeUser.xCopyTo<Person>();
            model.Watchers = new List<Person>();
            foreach (var user in watchers.Where(k => k.Status == (int)GeneralStatus.Active).ToList().Select(m => m.Users))
            {
                model.Watchers.Add(user.xCopyTo<Person>());
            }

            model.Reporter = createdUser.xCopyTo<Person>();
            model.TicketIdx = Idx;
            return model;
        }

        public async Task<TicketsModel> GetTickets(int Idx)
        {
            var ticket = await _ticketRepository.GetByIdAsync(Idx);
            return ticket.xCopyTo<TicketsModel>();
        }
        public async Task<List<TicketCommentsModel>> GetTicketComments(int Idx)
        {
            var ticketComments = await _ticketRepository.GetTicketComments(Idx);
            List<TicketCommentsModel> model = new List<TicketCommentsModel>();
            foreach (var comment in ticketComments)
            {
                var commentModel = new TicketCommentsModel
                {
                    Comment = comment.Comment,
                    Idx = comment.Idx,
                    InsertDate = comment.InsertDate,
                    UpdateDate = comment.UpdateDate,
                    InsertUserIdx = comment.InsertUserIdx,
                    InsertedUser = comment.User.xCopyTo<UserModel>()
                };
                foreach (var attachment in comment.TicketAttachments)
                {
                    commentModel.TicketAttachments.Add(attachment.xCopyTo<TicketAttachmentsModel>());
                }

                model.Add(commentModel);

            }
            return model;
        }

        public async Task<List<TicketLogTimesModel>> GetTicketLogTimes(int Idx)
        {
            var ticketLogTimes = await _ticketRepository.GetTicketLogTimes(Idx);
            List<TicketLogTimesModel> ticketLogTypesModels = new List<TicketLogTimesModel>();
            foreach (var logTime in ticketLogTimes)
            {
                TicketLogTimesModel ticketLog = logTime.xCopyTo<TicketLogTimesModel>();
                ticketLog.WorkingTypesModel = logTime.WorkingTypes.xCopyTo<WorkingTypesModel>();
                ticketLog.UserModel = logTime.User.xCopyTo<UserModel>();
                ticketLogTypesModels.Add(ticketLog);
            }

            return ticketLogTypesModels;
        }

        public async Task<List<TicketAttachmentsModel>> GetTicketAttachments(int TicketIdx)
        {
            var attachments = await _ticketRepository.GetTicketAttachments(TicketIdx);
            List<TicketAttachmentsModel> ticketAttachments = new List<TicketAttachmentsModel>();
            foreach (var attachment in attachments)
            {
                ticketAttachments.Add(new TicketAttachmentsModel
                {
                    AttachmentName = attachment.AttachmentName,
                    AttachmentThumb = attachment.AttachmentThumb,
                    AttachmentUrl = attachment.AttachmentUrl,
                    CommentIdx = attachment.CommentIdx,
                    FileType = attachment.FileType,
                    TicketIdx = attachment.TicketIdx,
                    Idx = attachment.Idx
                });
            }
            return ticketAttachments;
        }

        public async Task<int> AddTicketComment(TicketCommentsModel model)
        {
            var comment = new TicketComments
            {
                Comment = model.Comment,
                InsertDate = DateTime.Now,
                TicketIdx = model.TicketIdx,
                UserIdx = model.InsertUserIdx,
                InsertUserIdx = model.InsertUserIdx
            };

            comment = await _ticketCommentRepository.AddAsync(comment);

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                await SaveAttachments(model.Attachments.ToList(), model.TicketIdx, model.InsertUserIdx, comment.Idx);
            }

            if (model.SendMailtoCustomer)
            {
                Tickets ticket = await _ticketRepository.GetByIdAsync(model.TicketIdx);

                var IsItCreatedFromMail = await _ticketRepository.GetMailTicketsByThreadId(ticket.TicketNumber);


                var ticketFirm = await _firmRepository.GetByIdAsync(ticket.FirmIdx);

                SendMailModel sendMailModel = new SendMailModel()
                {
                    Headers = new RFC2822Header()
                    {
                        Date = DateTime.Now.ToString(),
                        DeliveredTo = IsItCreatedFromMail.From,
                        ErrorsTo = "talep@CMS.com.tr",
                        From = "talep@CMS.com.tr",
                        Subject = ticket.Name,
                        To = IsItCreatedFromMail.From,
                    },
                    BodyHtml = model.Comment,
                    BodyText = model.Comment,
                    Subject = ticket.Name,
                    Summary = ticket.Summary,
                    ToRecipients = new List<string>()
                    {
                        ticketFirm.Email
                    },
                    UserId = "me"
                };



                if (IsItCreatedFromMail != null && IsItCreatedFromMail.Idx > 0)
                {
                    sendMailModel.ThreadId = IsItCreatedFromMail.ThreadId;
                    sendMailModel.ToRecipients.Add(IsItCreatedFromMail.From);

                    var headersList = JsonConvert.DeserializeObject<List<HeaderKeyValue>>(IsItCreatedFromMail.Headers);

                    var MailReferencesHeaderTag = headersList.Where(w => w.Key == "References").FirstOrDefault();
                    if (MailReferencesHeaderTag != null)
                        sendMailModel.Headers.References = MailReferencesHeaderTag.Value;

                    //In-Reply-To
                    var MailInReplyToHeaderTag = headersList.Where(w => w.Key == "Message-ID").FirstOrDefault();
                    if (MailInReplyToHeaderTag != null)
                        sendMailModel.Headers.InReplyTo = MailInReplyToHeaderTag.Value;

                }

                // if (model.Attachments != null && model.Attachments.Count() > 0)
                // {
                //     sendMailModel.Attachments = new List<Attachment>();

                //     foreach (var attachment in model.Attachments)
                //     {
                //         sendMailModel.Attachments.Add(new Attachment()
                //         {
                //             ByteData = attachment.
                //         });

                //     }
                // }
                try
                {
                    _logger.LogDebug("ticket created from mail {data}", JsonConvert.SerializeObject(IsItCreatedFromMail));

                    _logger.LogInformation("Mail gidecek data : {data}", JsonConvert.SerializeObject(sendMailModel));
                }
                catch (Exception ex)
                {
                }

                //ThreadId set et.


                int result = await SendTicketReply(sendMailModel);


            }


            return comment.Idx;
        }

        public async Task<int> AddTicketLogTimes(TicketLogTimesModel ticketLogTimesModel)
        {
            TicketLogTimes ticketLogTimes = ticketLogTimesModel.xCopyTo<TicketLogTimes>();

            if (ticketLogTimesModel.Idx > 0)
            {
                await _ticketLogTimesRepository.UpdateAsync(ticketLogTimes);
            }
            else
            {
                ticketLogTimes.UserIdx = ticketLogTimesModel.InsertUserIdx;
                ticketLogTimes = await _ticketLogTimesRepository.AddAsync(ticketLogTimes);
            }

            return ticketLogTimes.Idx;
        }

        public async Task<int> ChangeTicketState(TicketChangeStateModel changeStateModel)
        {
            Tickets ticket = await _ticketRepository.GetByIdAsync(changeStateModel.TicketIdx);

            TicketHistories ticketHistories = new TicketHistories
            {
                UserIdx = changeStateModel.UserIdx,
                InsertDate = DateTime.Now,
                InsertUserIdx = changeStateModel.UserIdx,
                CaptureNow = JsonConvert.SerializeObject(ticket),
                CurrentAssigneeUserIdx = ticket.AssigneeUserIdx,
                CurrentTicketStatus = ticket.Status,
                Description = changeStateModel.Description,
                NewAssigneeUserIdx = ticket.AssigneeUserIdx,
                NewTicketStatus = changeStateModel.StatusIdx,
                TicketIdx = changeStateModel.TicketIdx
            };

            await _ticketHistoriesRepository.AddAsync(ticketHistories);

            ticket.Status = changeStateModel.StatusIdx;
            ticket.UpdateDate = DateTime.Now;
            ticket.UpdateUserIdx = changeStateModel.UserIdx;
            ticket.TypeIdx = changeStateModel.TypeIdx.GetValueOrDefault();
            await _ticketRepository.UpdateAsync(ticket);

            return ticket.Idx;

        }

        public async Task<int> ChangeTicketAssiggnee(TicketAssigneeModel ticketAssiggnee)
        {
            Tickets ticket = await _ticketRepository.GetByIdAsync(ticketAssiggnee.TicketIdx);

            TicketHistories ticketHistories = new TicketHistories
            {
                UserIdx = ticketAssiggnee.UserIdx,
                InsertDate = DateTime.Now,
                InsertUserIdx = ticketAssiggnee.UserIdx,
                CaptureNow = JsonConvert.SerializeObject(ticket),
                CurrentAssigneeUserIdx = ticket.AssigneeUserIdx,
                CurrentTicketStatus = ticket.Status,
                Description = ticketAssiggnee.Description,
                NewAssigneeUserIdx = ticketAssiggnee.AssigneeUserIdx,
                NewTicketStatus = ticket.Status,
                TicketIdx = ticketAssiggnee.TicketIdx
            };

            await _ticketHistoriesRepository.AddAsync(ticketHistories);

            ticket.AssigneeUserIdx = ticketAssiggnee.AssigneeUserIdx;
            ticket.UpdateDate = DateTime.Now;
            ticket.UpdateUserIdx = ticketAssiggnee.UserIdx;

            await _ticketRepository.UpdateAsync(ticket);

            return ticket.Idx;

        }

        public async Task<int> ChangeTicketDescription(TicketDescriptionModel ticketDescriptionModel)
        {
            Tickets ticket = await _ticketRepository.GetByIdAsync(ticketDescriptionModel.TicketIdx);


            TicketHistories ticketHistories = new TicketHistories
            {
                UserIdx = ticketDescriptionModel.UserIdx,
                InsertDate = DateTime.Now,
                InsertUserIdx = ticketDescriptionModel.UserIdx,
                CaptureNow = JsonConvert.SerializeObject(ticket),
                CurrentAssigneeUserIdx = ticket.AssigneeUserIdx,
                CurrentTicketStatus = ticket.Status,
                Description = ticketDescriptionModel.Description,
                NewAssigneeUserIdx = ticket.AssigneeUserIdx,
                NewTicketStatus = ticket.Status,
                TicketIdx = ticketDescriptionModel.TicketIdx
            };

            await _ticketHistoriesRepository.AddAsync(ticketHistories);

            ticket.Description = ticketDescriptionModel.Description;
            ticket.UpdateDate = DateTime.Now;
            ticket.UpdateUserIdx = ticketDescriptionModel.UserIdx;

            await _ticketRepository.UpdateAsync(ticket);

            return ticket.Idx;
        }

        public async Task<List<TicketHistoriesModel>> GetTicketHistories(int Idx)
        {
            var ticketHistories = await _ticketRepository.GetTicketHistories(Idx);

            List<TicketHistoriesModel> ticketHistoriesModels = new List<TicketHistoriesModel>();

            foreach (var histories in ticketHistories)
            {
                TicketHistoriesModel ticketHistoriesModel = histories.xCopyTo<TicketHistoriesModel>();
                ticketHistoriesModel.User = histories.User.xCopyTo<UserModel>();
                ticketHistoriesModel.CurrentAssigneeUser = (await _userRepository.GetByIdAsync(histories.CurrentAssigneeUserIdx)).xCopyTo<UserModel>();
                ticketHistoriesModel.NewAssigneeUser = (await _userRepository.GetByIdAsync(histories.NewAssigneeUserIdx)).xCopyTo<UserModel>();
                ticketHistoriesModel.CurrentTicketStatusModel = (await _ticketStatusRepository.GetByIdAsync(histories.CurrentTicketStatus)).xCopyTo<TicketStatusModel>();
                ticketHistoriesModel.NewTicketStatusModel = (await _ticketStatusRepository.GetByIdAsync(histories.NewTicketStatus)).xCopyTo<TicketStatusModel>();
                ticketHistoriesModels.Add(ticketHistoriesModel);
            }

            return ticketHistoriesModels;

        }

        public async Task<int> ReopenTicket(TicketReopenModel ticketReopenModel)
        {
            Tickets ticket = await _ticketRepository.GetByIdAsync(ticketReopenModel.TicketIdx);


            TicketHistories ticketHistories = new TicketHistories
            {
                UserIdx = ticketReopenModel.UserIdx,
                InsertDate = DateTime.Now,
                InsertUserIdx = ticketReopenModel.UserIdx,
                CaptureNow = JsonConvert.SerializeObject(ticket),
                CurrentAssigneeUserIdx = ticket.AssigneeUserIdx,
                CurrentTicketStatus = ticket.Status,
                Description = ticketReopenModel.Description,
                NewAssigneeUserIdx = ticket.AssigneeUserIdx,
                NewTicketStatus = (int)ITicketStatus.REOPENED,
                TicketIdx = ticketReopenModel.TicketIdx
            };

            await _ticketHistoriesRepository.AddAsync(ticketHistories);

            ticket.UpdateDate = DateTime.Now;
            ticket.UpdateUserIdx = ticketReopenModel.UserIdx;
            ticket.Status = (int)ITicketStatus.REOPENED;
            ticket.PriorityIdx = ticketReopenModel.PriorityIdx;

            await _ticketRepository.UpdateAsync(ticket);

            if (ticketReopenModel.Attachments != null && ticketReopenModel.Attachments.Length > 0)
            {
                await SaveAttachments(ticketReopenModel.Attachments.ToList(), ticketReopenModel.TicketIdx, ticketReopenModel.UserIdx);
            }

            return ticket.Idx;
        }

        public async Task<TicketLogTimesModel> GetTicketLogTime(int Idx)
        {
            var logTime = await _ticketLogTimesRepository.GetByIdAsync(Idx);
            return logTime.xCopyTo<TicketLogTimesModel>();
        }

        public async Task<List<TicketLabelsModel>> GetTicketLabels()
        {
            var labels = await _labelRepository.ListAllAsync();
            List<TicketLabelsModel> ticketLabels = new List<TicketLabelsModel>();
            foreach (var label in labels)
            {
                ticketLabels.Add(label.xCopyTo<TicketLabelsModel>());
            }
            return ticketLabels;
        }

        public async Task<int> AddTicketLabels(TicketLabelsChangeModel ticketLabelsModel)
        {
            var tickets = await _ticketRepository.GetTicketDetails(ticketLabelsModel.TicketIdx);
            var ticketLabels = new List<TicketLabels>();
            ticketLabels = tickets.TicketLabels.ToList();
            foreach (var labels in ticketLabels)
            {
                await _ticketLabelRepository.DeleteAsync(labels);
            }

            tickets.TicketLabels = new List<TicketLabels>();
            foreach (var ticketLabel in ticketLabelsModel.SelectedLabel)
            {
                tickets.TicketLabels.Add(new TicketLabels
                {
                    InsertDate = DateTime.Now,
                    InsertUserIdx = ticketLabelsModel.UserIdx,
                    LabelIdx = ticketLabel,
                    TicketIdx = ticketLabelsModel.TicketIdx
                });
            }

            tickets.UpdateDate = DateTime.Now;
            tickets.UpdateUserIdx = ticketLabelsModel.UserIdx;

            await _ticketRepository.UpdateAsync(tickets);

            return tickets.Idx;

        }

        public async Task<DataTableResponse<TicketLabelsModel>> GetTicketLabels(IDataTableRequest request)
        {
            var labels = await _ticketRepository.GetTicketLabels(request);

            DataTableResponse<TicketLabelsModel> returnModel = new DataTableResponse<TicketLabelsModel>();
            returnModel.Draw = labels.Draw;
            returnModel.RecordsFiltered = labels.RecordsFiltered;
            returnModel.RecordsTotal = labels.RecordsTotal;

            returnModel.Data = labels.Data.Select(f =>
            new TicketLabelsModel()
            {
                Idx = f.Idx,
                Name_TR = f.Label.Name_TR,

            }).ToList();

            return returnModel;

        }

        public async Task<int> ChangeTicketInfos(TicketChangeInfoModel ticketChangeInfoModel)
        {
            Tickets ticket = await _ticketRepository.GetByIdAsync(ticketChangeInfoModel.Idx);


            TicketHistories ticketHistories = new TicketHistories
            {
                UserIdx = ticketChangeInfoModel.UserIdx,
                InsertDate = DateTime.Now,
                InsertUserIdx = ticketChangeInfoModel.UserIdx,
                CaptureNow = JsonConvert.SerializeObject(ticket),
                CurrentAssigneeUserIdx = ticket.AssigneeUserIdx,
                CurrentTicketStatus = ticket.Status,
                Description = ticket.Description,
                NewAssigneeUserIdx = ticket.AssigneeUserIdx,
                NewTicketStatus = ticket.Status,
                TicketIdx = ticketChangeInfoModel.Idx,

            };

            await _ticketHistoriesRepository.AddAsync(ticketHistories);

            ticket.PriorityIdx = ticketChangeInfoModel.PriorityIdx;
            ticket.TypeIdx = ticketChangeInfoModel.TypeIdx;
            ticket.FirmIdx = ticketChangeInfoModel.FirmIdx;
            ticket.FirmUserIdx = ticketChangeInfoModel.FirmUserIdx;
            ticket.EstimatedTime = ticketChangeInfoModel.EstimatedTime;
            ticket.UpdateDate = DateTime.Now;
            ticket.UpdateUserIdx = ticketChangeInfoModel.UserIdx;

            await _ticketRepository.UpdateAsync(ticket);

            return ticket.Idx;
        }

        public async Task<int> AddTicketWatchers(TicketWatchersModel ticketWatchersModel)
        {
            TicketPeoplesModel ticketPeoples = await GetTicketPeoples(ticketWatchersModel.TicketIdx);
            //List<Person> watchers = new List<Person>();
            //watchers = ticketPeoples.Watchers;
            //foreach (var watcher in watchers)
            //{
            //    TicketWatchers ticketWatchers = await _ticketWatchersRepository.GetByIdAsync(watcher.Idx);                
            //    await _ticketWatchersRepository.DeleteAsync(ticketWatchers);
            //}

            foreach (var watcher in ticketWatchersModel.SelectedUsers)
            {
                if (!ticketPeoples.Watchers.Any(r => r.Idx == Convert.ToInt32(watcher)))
                {
                    TicketWatchers ticketWatchers = new TicketWatchers
                    {
                        TicketIdx = ticketWatchersModel.TicketIdx,
                        InsertDate = DateTime.Now,
                        InsertUserIdx = ticketWatchersModel.UserIdx,
                        UserIdx = Convert.ToInt32(watcher),
                        Status = (int)GeneralStatus.Active,
                    };
                    await _ticketWatchersRepository.AddAsync(ticketWatchers);
                }

            }
            IEnumerable<string> except = ticketPeoples.Watchers.Select(r => r.Idx.ToString()).Except(ticketWatchersModel.SelectedUsers.ToList());
            foreach (var UserId in except)
            {
                TicketWatchers watcher = await _ticketRepository.GetTicketWatchers(Convert.ToInt32(UserId), ticketWatchersModel.TicketIdx);
                watcher.Status = (int)GeneralStatus.Passsive;
                watcher.UpdateDate = DateTime.Now;
                watcher.UpdateUserIdx = ticketWatchersModel.UserIdx;
                await _ticketWatchersRepository.UpdateAsync(watcher);
            }

            return ticketWatchersModel.TicketIdx;
        }

        public async Task<int> AddTicketEvents(TicketEventsModel ticketEventsModel)
        {
            TicketEvents ticketEvents = ticketEventsModel.xCopyTo<TicketEvents>();
            await _ticketEventsRepository.AddAsync(ticketEvents);
            return ticketEvents.Idx;
        }

        public async Task<TicketTypesModel> GetTicketType(int TicketIdx)
        {
            var tickettype = await _ticketRepository.GetTicketType(TicketIdx);
            return tickettype.xCopyTo<TicketTypesModel>();
        }

        public async Task<List<TicketRelatedIssues>> GetTicketRelateds(int TicketIdx)
        {
            var relatedTickets = await _ticketRepository.GetTicketRelateds(TicketIdx);
            List<TicketRelatedIssues> ticketRelatedIssues = new List<TicketRelatedIssues>();
            foreach (var ticketRelated in relatedTickets)
            {
                TicketRelatedIssues relatedIssues = new TicketRelatedIssues();
                relatedIssues.Idx = ticketRelated.Idx;
                relatedIssues.TicketIdx = ticketRelated.TicketIdx;
                relatedIssues.RelatedTicketIdx = ticketRelated.RelatedTicketIdx;
                relatedIssues.RelatedTicket = ticketRelated.RelatedTicket.xCopyTo<TicketsModel>();
                relatedIssues.TicketStatus = ticketRelated.RelatedTicket.TicketStatus.xCopyTo<TicketStatusModel>();
                if (ticketRelated.RelatedTicket.AssigneeUserIdx > 0)
                {
                    var assigneeUser = await _userRepository.GetByIdAsync(ticketRelated.RelatedTicket.AssigneeUserIdx);
                    relatedIssues.AssigneeUser = assigneeUser.xCopyTo<Person>();
                }
                else
                {
                    relatedIssues.AssigneeUser = new Person { FirstName = "*", LastName = "*" };
                }

                ticketRelatedIssues.Add(relatedIssues);
            }
            return ticketRelatedIssues;
        }

        public async Task<List<TicketsModel>> GetTicketsByFirm(int FirmIdx)
        {
            var tickets = await _ticketRepository.GetTicketsByFirm(FirmIdx);
            List<TicketsModel> ticketsModels = new List<TicketsModel>();
            foreach (var ticket in tickets)
            {
                ticketsModels.Add(ticket.xCopyTo<TicketsModel>());
            }
            return ticketsModels;
        }

        public async Task<int> AddTicketRelateds(TicketRelatedIssues ticketRelatedModel)
        {
            TicketRelatedTickets ticketRelatedTickets = new TicketRelatedTickets
            {
                InsertUserIdx = ticketRelatedModel.UserIdx,
                InsertDate = DateTime.Now,
                RelatedTicketIdx = ticketRelatedModel.RelatedTicketIdx,
                TicketIdx = ticketRelatedModel.TicketIdx,
                Status = (int)GeneralStatus.Active
            };
            var res = await _ticketRelatedsRepository.AddAsync(ticketRelatedTickets);
            return res.Idx;
        }

        public async Task<int> DeleteTicketRelateds(int Idx)
        {
            try
            {
                var relatedTicket = await _ticketRelatedsRepository.GetByIdAsync(Idx);
                await _ticketRelatedsRepository.DeleteAsync(relatedTicket);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<int> SendTicketReply(SendMailModel model)
        {
            try
            {
                _ticketIntegrationFactory.SetIntegrationType(TypeOfTicketIntegration.Google);

                var integration = _ticketIntegrationFactory.GetIntegration();

                integration.SendTicketReplyById(model);
                _logger.LogInformation("Mail gitti {data}", JsonConvert.SerializeObject(model));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "HATA {msg}", ex.Message);

            }
            return 1;
        }

        public async Task<TicketCountsModel> GetAllTicketsCounts(int AssigneeUserIdx)
        {
            TicketCountsModel returnModel = new TicketCountsModel();
            try
            {
                DTParameterModel requestModel = new DTParameterModel() { Length = 1 };

                var myTicketEntity = _ticketRepository.GetAllMyTickets(AssigneeUserIdx, requestModel);
                var myDoneTicketEntity = _ticketRepository.GetAllMyDoneTickets(AssigneeUserIdx, requestModel);
                var allOpenTicketEntity = _ticketRepository.GetAllOpenTickets(requestModel);
                var reportedMeTicketEntity = _ticketRepository.GetReportedMeTickets(AssigneeUserIdx, requestModel);

                var result = await Task.WhenAll(
                    myTicketEntity,
                    myDoneTicketEntity,
                    allOpenTicketEntity,
                    reportedMeTicketEntity);

                returnModel.OpenedIssueOnMeCount = result[0].RecordsTotal;
                returnModel.ClosedIssueByMeCount = result[1].RecordsTotal;
                returnModel.OpenedAllIssueCount = result[2].RecordsTotal;
                returnModel.CreatedIssueByMeCount = result[3].RecordsTotal;

                return returnModel;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "HATA {msg}", ex.Message);
            }

            return returnModel;
        }

        public async Task<bool> DeleteAttachment(int Idx)
        {
            try
            {
                var attachment = await _ticketAttachmentRepository.GetByIdAsync(Idx);
                attachment.Status = (int)GeneralStatus.Passsive;
                await _ticketAttachmentRepository.UpdateAsync(attachment);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

