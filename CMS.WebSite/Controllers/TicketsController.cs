using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.WebSite.Filters;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Controllers
{
    public class TicketsController : Controller
    {

        ITicketService _ticketService;
        IStringLocalizer<SharedResources> _localizer;
        IDefinitionService _definitionService;
        public TicketsController(ITicketService ticketService, IStringLocalizer<SharedResources> localizer, IDefinitionService definitionService)
        {
            _ticketService = ticketService;
            _localizer = localizer;
            _definitionService = definitionService;

        }
        [HasPermission(Permissions.Read_Ticket)]
        public IActionResult Index()
        {
            var v = _localizer["Save"];


            return View();
        }
        [HasPermission(Permissions.Create_Ticket)]
        [HttpGet]
        public IActionResult Create()
        {
            CreateTicketModel createTicketModel = new CreateTicketModel();
            return PartialView(createTicketModel);
        }
        [HasPermission(Permissions.Read_Ticket)]
        public IActionResult Ticket(int Idx)
        {
            return View(Idx);
        }
        [HasPermission(Permissions.Read_Ticket)]
        public IActionResult TicketsDatatable(int Id)
        {
            TicketDatatableModel ticketDatatableModel = new TicketDatatableModel();
            switch (Id)
            {
                case 1:
                    ticketDatatableModel.CallBackUrl = Url.Action("_MyOpenTicketsCallback", "Tickets");
                    ticketDatatableModel.EditUrl = Url.Action("Ticket", "Tickets");
                    ticketDatatableModel.CustomUrl = Url.Action("Ticket", "Tickets");
                    break;
                case 2:
                    ticketDatatableModel.CallBackUrl = Url.Action("_ReportedMeTicketsCallback", "Tickets");
                    ticketDatatableModel.EditUrl = Url.Action("Ticket", "Tickets");
                    ticketDatatableModel.CustomUrl = Url.Action("Ticket", "Tickets");
                    break;
                case 3:
                    ticketDatatableModel.CallBackUrl = Url.Action("_MyDoneTicketsCallback", "Tickets");
                    ticketDatatableModel.EditUrl = Url.Action("Ticket", "Tickets");
                    ticketDatatableModel.CustomUrl = Url.Action("Ticket", "Tickets");
                    break;
                case 4:
                    ticketDatatableModel.CallBackUrl = Url.Action("_AllOpenTicketsCallback", "Tickets");
                    ticketDatatableModel.EditUrl = Url.Action("Ticket", "Tickets");
                    ticketDatatableModel.CustomUrl = Url.Action("Ticket", "Tickets");
                    break;
                case 5:
                    ticketDatatableModel.CallBackUrl = Url.Action("_AllTicketsCallback", "Tickets");
                    ticketDatatableModel.EditUrl = Url.Action("Ticket", "Tickets");
                    ticketDatatableModel.CustomUrl = Url.Action("Ticket", "Tickets");
                    break;
                default:
                    break;
            }
            return PartialView(ticketDatatableModel);
        }
        [HasPermission(Permissions.Read_Ticket)]
        public async Task<IActionResult> _Tickets(string filter)
        {
            return Json(await _ticketService.AllTickets());
        }


        [HasPermission(Permissions.Read_Ticket)]
        [HttpPost]
        public async Task<IActionResult> _MyOpenTicketsCallback(DTParameterModel dataTableRequest)
        {
            int AssiggneeUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(await _ticketService.GetAllMyActiveTickets(AssiggneeUserIdx, dataTableRequest));
        }

        [HasPermission(Permissions.Read_Ticket)]
        [HttpPost]
        public async Task<IActionResult> _MyDoneTicketsCallback(DTParameterModel dataTableRequest)
        {
            int AssiggneeUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(await _ticketService.GetAllMyDoneTickets(AssiggneeUserIdx, dataTableRequest));
        }

        [HasPermission(Permissions.Read_Ticket)]
        [HttpPost]
        public async Task<IActionResult> _AllOpenTicketsCallback(DTParameterModel dataTableRequest)
        {
            return Json(await _ticketService.GetAllOpenTickets(dataTableRequest));
        }
        [HasPermission(Permissions.Read_Ticket)]
        [HttpPost]
        public async Task<IActionResult> _ReportedMeTicketsCallback(DTParameterModel dataTableRequest)
        {
            int InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(await _ticketService.GetReportedMeTickets(InsertUserIdx, dataTableRequest));
        }
        [HasPermission(Permissions.Read_Ticket)]
        [HttpPost]
        public async Task<IActionResult> _AllTicketsCallback(DTParameterModel dataTableRequest)
        {
            return Json(await _ticketService.GetAllTickets(dataTableRequest));
        }
        [HttpPost]
        [HasPermission(Permissions.Create_Ticket)]
        public async Task<IActionResult> Create(CreateTicketModel createTicketModel)
        {
            if (ModelState.IsValid)
            {
                createTicketModel.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var Id = await _ticketService.CreateTicket(createTicketModel);
                var redirectUrl = Url.Action("Index", "Tickets");
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır.", ReturnUrl = redirectUrl });
            }
            return PartialView(createTicketModel);
        }

        public ActionResult UploadFiles()
        {
            if (Request.Form.Files.Count() > 0)
            {
                
            }

            return Json("Ok");
        }

        //[HttpPost]
        //public async Task<IActionResult> _TicketLogTimesCallBack(int Idx, DTParameterModel dataTableRequest)
        //{

        //    return Json(await _ticketService.GetTicketLogTimes(Idx, dataTableRequest));
        //}
        [HttpGet]
        [HasPermission(Permissions.Comment_Ticket)]
        public async Task<IActionResult> AddTicketComment(int Id)
        {
            TicketCommentsModel model = new TicketCommentsModel
            {
                TicketIdx = Id,
                InsertUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                InsertDate = DateTime.Now
            };
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Comment_Ticket)]
        public async Task<IActionResult> AddTicketComment(TicketCommentsModel ticketCommentsModel)
        {
            if (ModelState.IsValid)
            {
                var commentIdx = await _ticketService.AddTicketComment(ticketCommentsModel);
                if (ticketCommentsModel.SendMailtoCustomer)
                {
                    TicketEventsModel ticketEvents = new TicketEventsModel
                    {
                        ContentIdx = commentIdx,
                        EventTypeIdx = 1,
                        Status = (int)GeneralStatus.Active,
                        MailisSended = 0,
                        TicketIdx = ticketCommentsModel.TicketIdx,
                        InsertUserIdx = ticketCommentsModel.InsertUserIdx
                    };
                    await _ticketService.AddTicketEvents(ticketEvents);
                }
                return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
            }
            else
            {
                return PartialView(ticketCommentsModel);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.LogTime_Ticket)]
        public async Task<IActionResult> AddTicketLogTimes(int Id, int LogId = 0)
        {
            TicketLogTimesModel ticketLogTimesModel = new TicketLogTimesModel
            {
                TicketIdx = Id,
                InsertUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                InsertDate = DateTime.Now
            };

            if (LogId > 0)
            {
                var logTime = await _ticketService.GetTicketLogTime(LogId);
                ticketLogTimesModel.Idx = logTime.Idx;
                ticketLogTimesModel.InsertDate = logTime.InsertDate;
                ticketLogTimesModel.InsertUserIdx = logTime.InsertUserIdx;
                ticketLogTimesModel.Comment = logTime.Comment;
                ticketLogTimesModel.LogTime = logTime.LogTime;
                ticketLogTimesModel.UserIdx = logTime.UserIdx;
                ticketLogTimesModel.WorkingTypeIdx = logTime.WorkingTypeIdx;
                ticketLogTimesModel.UpdateUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            return PartialView(ticketLogTimesModel);

        }

        [HttpPost]
        [HasPermission(Permissions.LogTime_Ticket)]
        public async Task<IActionResult> AddTicketLogTimes(TicketLogTimesModel ticketLogTimesModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.AddTicketLogTimes(ticketLogTimesModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketLogTimesModel);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Assignee_Ticket)]
        public async Task<IActionResult> ChangeAssigneeUserIdx(int Id)
        {
            var ticket = await _ticketService.GetTickets(Id);
            TicketAssigneeModel ticketAssigneeModel = new TicketAssigneeModel
            {
                UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TicketIdx = Id,
                AssigneeUserIdx = ticket.AssigneeUserIdx
            };
            return PartialView(ticketAssigneeModel);
        }
        [HttpPost]
        [HasPermission(Permissions.Assignee_Ticket)]
        public async Task<ActionResult> ChangeAssigneeUserIdx(TicketAssigneeModel ticketAssigneeModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.ChangeTicketAssiggnee(ticketAssigneeModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketAssigneeModel);
            }

        }

        [HttpGet]
        [HasPermission(Permissions.ChangeState_Ticket)]
        public async Task<IActionResult> ChangeTicketState(int Id)
        {
            var ticket = await _ticketService.GetTickets(Id);
            TicketChangeStateModel ticketAssigneeModel = new TicketChangeStateModel
            {
                UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TicketIdx = Id,
                StatusIdx = ticket.Status,
                TypeIdx = ticket.TypeIdx
            };
            return PartialView(ticketAssigneeModel);
        }
        [HttpPost]
        [HasPermission(Permissions.ChangeState_Ticket)]
        public async Task<ActionResult> ChangeTicketState(TicketChangeStateModel ticketChangeStateModel)
        {
            if (ModelState.IsValid)
            {
                // Buraya related t,cketlaaın durumu kontrol edilecek.
                // Firma seçili değilse güncellenmesi istenecek.
                // Ticketin kategorisi yoksa onun kontrolu sağlanacak Zorunlu hale getirilecek. 
                // 

                await _ticketService.ChangeTicketState(ticketChangeStateModel);

                var statusModel = await _definitionService.GetTicketStatu(ticketChangeStateModel.StatusIdx);

                if (ticketChangeStateModel.SendMailtoCustomer || statusModel.CategoryIdx == ((int)ITicketStatusCategory.DONE))
                {
                    TicketEventsModel ticketEvents = new TicketEventsModel
                    {
                        ContentIdx = ticketChangeStateModel.TicketIdx,
                        EventTypeIdx = 2,
                        Status = (int)GeneralStatus.Active,
                        MailisSended = 0,
                        TicketIdx = ticketChangeStateModel.TicketIdx,
                        InsertUserIdx = ticketChangeStateModel.UserIdx
                    };
                    await _ticketService.AddTicketEvents(ticketEvents);
                }
                return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
            }
            else
            {
                return PartialView(ticketChangeStateModel);
            }

        }
        [HttpPost]
        [HasPermission(Permissions.ChangeState_Ticket)]
        public async Task<ActionResult> CheckTicketClosable(int StatusIdx, int TicketIdx)
        {
            var stateCategory = await _definitionService.GetTicketStatu(StatusIdx);
            if (stateCategory.CategoryIdx == (int)ITicketStatusCategory.DONE)
            {
                var relatedTickets = await _ticketService.GetTicketRelateds(TicketIdx);
                if (relatedTickets.Any(r => r.TicketStatus.CategoryIdx != ((int)ITicketStatusCategory.DONE)))
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = "İlişkili işler tamamlanmadığı için ticket kapatılamaz.", ExtraData = ((int)ITicketStatusCategory.DONE).ToString() });
                }
                return Json(new JsonResponse { IsSuccess = true, Message = "Kategori zorunludur.", ExtraData = ((int)ITicketStatusCategory.DONE).ToString() });
            }
            return Json(new JsonResponse { IsSuccess = true, ExtraData = stateCategory.CategoryIdx.ToString() });
        }

        [HttpGet]
        [HasPermission(Permissions.Change_TicketInfos)]
        public async Task<IActionResult> ChangeTicketInfos(int Id)
        {
            var ticket = await _ticketService.GetTickets(Id);
            TicketChangeInfoModel ticketChangeInfoModel = new TicketChangeInfoModel
            {
                Idx = ticket.Idx,
                FirmIdx = ticket.FirmIdx,
                FirmUserIdx = ticket.FirmUserIdx.GetValueOrDefault(),
                PriorityIdx = ticket.PriorityIdx,
                TypeIdx = ticket.TypeIdx.GetValueOrDefault(),
                EstimatedTime = ticket.EstimatedTime,
            };
            return PartialView(ticketChangeInfoModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Change_TicketInfos)]
        public async Task<IActionResult> ChangeTicketInfos(TicketChangeInfoModel ticketChangeInfoModel)
        {
            if (ModelState.IsValid)
            {
                int id = await _ticketService.ChangeTicketInfos(ticketChangeInfoModel);
                if (id > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = "İşlem Başarısızdır." });
                }

            }
            else
            {
                return PartialView(ticketChangeInfoModel);
            }
        }
        [HttpPost]
        [HasPermission(Permissions.ChangeDescription_Ticket)]
        public async Task<ActionResult> ChangeTicketDescription(TicketDescriptionModel ticketChangeStateModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.ChangeTicketDescription(ticketChangeStateModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketChangeStateModel);
            }

        }

        [HttpGet]
        [HasPermission(Permissions.ChangeLabel_Ticket)]
        public async Task<IActionResult> ChangeTicketLabels(int Id)
        {
            TicketsModel ticketModel = await _ticketService.GetTickets(Id);
            TicketDescriptionModel ticketDescriptionModel = new TicketDescriptionModel
            {
                UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TicketIdx = Id,
                Description = ticketModel.Description
            };
            return PartialView(ticketDescriptionModel);
        }
        [HttpPost]
        [HasPermission(Permissions.ChangeLabel_Ticket)]
        public async Task<ActionResult> ChangeTicketLabels(TicketDescriptionModel ticketChangeStateModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.ChangeTicketDescription(ticketChangeStateModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketChangeStateModel);
            }

        }

        [HttpGet]
        [HasPermission(Permissions.Reopen_Ticket)]
        public async Task<IActionResult> ReOpenTicket(int Id)
        {

            TicketReopenModel ticketDescriptionModel = new TicketReopenModel
            {
                UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TicketIdx = Id,
            };
            return PartialView(ticketDescriptionModel);
        }
        [HttpPost]
        [HasPermission(Permissions.Reopen_Ticket)]
        public async Task<ActionResult> ReOpenTicket(TicketReopenModel ticketReopenModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.ReopenTicket(ticketReopenModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketReopenModel);
            }

        }


        [HttpGet]
        [HasPermission(Permissions.ChangeLabel_Ticket)]
        public async Task<IActionResult> ChangeLabelTicket(int Id)
        {
            TicketDetailsModel ticketModel = await _ticketService.GetTicketDetails(Id);
            TicketLabelsChangeModel ticketDescriptionModel = new TicketLabelsChangeModel
            {
                UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                TicketIdx = Id,
                SelectedLabel = ticketModel.TicketLabels.Select(r => r.Idx).ToArray(),
                TicketLabels = await _ticketService.GetTicketLabels()
            };
            return PartialView(ticketDescriptionModel);
        }
        [HttpPost]
        [HasPermission(Permissions.ChangeLabel_Ticket)]
        public async Task<ActionResult> ChangeLabelTicket(TicketLabelsChangeModel ticketLabelsChangeModel)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.AddTicketLabels(ticketLabelsChangeModel);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(ticketLabelsChangeModel);
            }

        }
        [HttpGet]
        [HasPermission(Permissions.Read_TicketWatchers)]
        public async Task<IActionResult> AddTicketWatcher(int Id)
        {
            TicketWatchersModel ticketWatchersModel = new TicketWatchersModel();
            TicketPeoplesModel ticketPeoplesModel = await _ticketService.GetTicketPeoples(Id);
            ticketWatchersModel.TicketIdx = Id;
            ticketWatchersModel.UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            ticketWatchersModel.SelectedUsers = ticketPeoplesModel.Watchers.Select(r => r.Idx.ToString()).ToArray();
            return PartialView(ticketWatchersModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_WorkingTypes)]
        public async Task<IActionResult> AddTicketWatcher(TicketWatchersModel model)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.AddTicketWatchers(model);
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return PartialView(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> AddRelatedTicket(int Id)
        {
            TicketRelatedIssues ticket = new TicketRelatedIssues();
            ticket.TicketIdx = Id;
            ticket.UserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return PartialView(ticket);
        }
        [HttpPost]
        public async Task<IActionResult> AddRelatedTicket(TicketRelatedIssues ticketRelated)
        {

            if (ModelState.IsValid)
            {
                var res = await _ticketService.AddTicketRelateds(ticketRelated);
                if (res > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = "İşlem Başarısızdır." });
                }

            }
            else
            {
                return PartialView(ticketRelated);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRelatedTicket(int Idx)
        {
            var res = await _ticketService.DeleteTicketRelateds(Idx);

            if (res > 0)
            {
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return Json(new JsonResponse { IsSuccess = false, Message = "İşlem Başarısızdır." });
            }
        }


        [HttpGet]
        [HasPermission(Permissions.Change_TicketInfos)]
        public async Task<IActionResult> DownloadAttachment(int Idx)
        {
            var fileData = await _ticketService.DownloadAttachment(Idx);

            return File(fileData.ByteData, "application/octet-stream", fileData.FileName);
        }
        [HttpPost]
        [HasPermission(Permissions.Change_TicketInfos)]
        public async Task<IActionResult> DeleteAttachment(int Idx)
        {
            var res = await _ticketService.DeleteAttachment(Idx);
            if (res)
            {
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
            }
            else
            {
                return Json(new JsonResponse { IsSuccess = false, Message = "İşlem Başarısızdır." });
            }
        }


        [HttpPost]
        [HasPermission(Permissions.Change_TicketInfos)]
        public async Task<IActionResult> UploadTicketAttachments(int TicketIdx)
        {
            if (Request.Form.Files.Count > 0)
            {
                List<IFormFile> attachments = new List<IFormFile>();
                foreach (var file in Request.Form.Files)
                {
                    attachments.Add(file);
                }

                var saveRes = await _ticketService.SaveAttachments(attachments, TicketIdx, Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            }
            return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır." });
        }
    }

}
