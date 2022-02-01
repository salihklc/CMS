using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Definition;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.WebSite.Filters;

namespace CMS.WebSite.Controllers
{
    public class DefinitionController : Controller
    {
        ITicketService _ticketService;
        IDefinitionService _definitionService;

        public DefinitionController(ITicketService ticketService, IDefinitionService definitionService)
        {
            _ticketService = ticketService;
            _definitionService = definitionService;

        }
        public IActionResult Index()
        {
            return View();
        }
        #region TicketLabels
        public IActionResult TicketLabels()
        {
            return View();
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_TicketLabel)]
        public async Task<IActionResult> EditTicketLabel(int Id)
        {
            var labelmodel = await _definitionService.GetLabel(Id);
            return PartialView(labelmodel);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_TicketLabel)]
        public async Task<IActionResult> EditTicketLabel(LabelModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.EditLabel(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }

        }
        [HttpGet]
        [HasPermission(Permissions.Create_TicketLabel)]
        public async Task<IActionResult> AddTicketLabel()
        {
            LabelModel model = new LabelModel();
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Create_TicketLabel)]
        public async Task<IActionResult> AddTicketLabel(LabelModel labelModel)
        {
            if (ModelState.IsValid)
            {
                labelModel.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddLabel(labelModel);
                if (labelIdx > 0)
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
                return PartialView(labelModel);
            }
        }

        [HasPermission(Permissions.Read_TicketLabel)]
        [HttpPost]
        public async Task<IActionResult> _TicketLabelsCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetLabels(dataTableRequest));
        }
        [HttpPost]
        [HasPermission(Permissions.Delete_TicketLabels)]
        public async Task<IActionResult> DeleteTicketLabel(int Idx)
        {
            try
            {
                var labelmodel = await _definitionService.GetLabel(Idx);
                labelmodel.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                labelmodel.UpdateDate = DateTime.Now;
                labelmodel.Status = (int)GeneralStatus.Passsive;
                int labelIdx = await _definitionService.EditLabel(labelmodel);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }

        }


        #endregion
        #region Priority
        public IActionResult TicketPriorities()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _TicketPrioritiesCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetPriorities(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_TicketPriorities)]
        public async Task<IActionResult> AddTicketPriority()
        {
            PriorityModel priorityModel = new PriorityModel();
            return PartialView(priorityModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_TicketPriorities)]
        public async Task<IActionResult> AddTicketPriority(PriorityModel model)
        {
            if (ModelState.IsValid)
            {
                model.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddPriority(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_TicketPriorities)]
        public async Task<IActionResult> EditTicketPriority(int Id)
        {
            var model = await _definitionService.GetPriority(Id);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_TicketPriorities)]
        public async Task<IActionResult> EditTicketPriority(PriorityModel priorityModel)
        {
            if (ModelState.IsValid)
            {
                priorityModel.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int Id = await _definitionService.EditPriority(priorityModel);
                if (Id > 0)
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
                return PartialView(priorityModel);
            }


        }

        [HttpPost]
        [HasPermission(Permissions.Delete_TicketPriorities)]
        public async Task<IActionResult> DeleteTicketPriority(int Idx)
        {
            try
            {
                var priority = await _definitionService.GetPriority(Idx);
                priority.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                priority.UpdateDate = DateTime.Now;
                priority.Status = (int)GeneralStatus.Passsive;
                int labelIdx = await _definitionService.EditPriority(priority);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }

        }
        #endregion
        #region TicketTypes
        public IActionResult TicketTypes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _TicketTypesCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetTicketTypes(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_TicketTypes)]
        public async Task<IActionResult> AddTicketTypes()
        {
            TicketTypesModel ticketTypesModel = new TicketTypesModel();
            return PartialView(ticketTypesModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_TicketTypes)]
        public async Task<IActionResult> AddTicketTypes(TicketTypesModel model)
        {
            if (ModelState.IsValid)
            {
                model.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddTicketType(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_TicketTypes)]
        public async Task<IActionResult> EditTicketTypes(int Id)
        {
            var model = await _definitionService.GetTicketTypes(Id);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_TicketTypes)]
        public async Task<IActionResult> EditTicketTypes(TicketTypesModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int Id = await _definitionService.EditTicketType(model);
                if (Id > 0)
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
                return PartialView(model);
            }


        }

        [HttpPost]
        [HasPermission(Permissions.Delete_TicketPriorities)]
        public async Task<IActionResult> DeleteTicketTypes(int Idx)
        {
            try
            {
                var tickettype = await _definitionService.GetTicketTypes(Idx);
                tickettype.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                tickettype.UpdateDate = DateTime.Now;
                tickettype.Status = (int)GeneralStatus.Passsive;
                int labelIdx = await _definitionService.EditTicketType(tickettype);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }

        }
        #endregion
        #region TicketStatusCategories
        public IActionResult TicketStatusCategories()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _TicketStatusCategoriesCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetTicketStatusCategories(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_TicketStatusCategories)]
        public async Task<IActionResult> AddTicketStatusCategories()
        {
            TicketStatusCategoriesModel ticketStatusCategoriesModel = new TicketStatusCategoriesModel();
            return PartialView(ticketStatusCategoriesModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_TicketStatusCategories)]
        public async Task<IActionResult> AddTicketStatusCategories(TicketStatusCategoriesModel model)
        {
            if (ModelState.IsValid)
            {
                model.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddTicketStatusCategory(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_TicketStatusCategories)]
        public async Task<IActionResult> EditTicketStatusCategories(int Id)
        {
            var model = await _definitionService.GetTicketStatusCategory(Id);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_TicketStatusCategories)]
        public async Task<IActionResult> EditTicketStatusCategories(TicketStatusCategoriesModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int Id = await _definitionService.EditTicketStatusCategory(model);
                if (Id > 0)
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
                return PartialView(model);
            }


        }

        [HttpPost]
        [HasPermission(Permissions.Delete_TicketStatusCategories)]
        public async Task<IActionResult> DeleteTicketStatusCategories(int Idx)
        {
            try
            {
                var statusCategory = await _definitionService.GetTicketStatusCategory(Idx);
                statusCategory.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                statusCategory.UpdateDate = DateTime.Now;
                statusCategory.Status = (int)GeneralStatus.Passsive;
                int labelIdx = await _definitionService.EditTicketStatusCategory(statusCategory);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }

        }
        #endregion
        #region TicketStatus
        public IActionResult TicketStatus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _TicketStatusCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetTicketStatus(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_TicketStatus)]
        public async Task<IActionResult> AddTicketStatus()
        {
            TicketStatusModel ticketStatusModel = new TicketStatusModel();
            return PartialView(ticketStatusModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_TicketStatus)]
        public async Task<IActionResult> AddTicketStatus(TicketStatusModel model)
        {
            if (ModelState.IsValid)
            {
                model.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddTicketStatus(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_TicketStatus)]
        public async Task<IActionResult> EditTicketStatus(int Id)
        {
            var model = await _definitionService.GetTicketStatu(Id);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_TicketStatus)]
        public async Task<IActionResult> EditTicketStatus(TicketStatusModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int Id = await _definitionService.EditTicketStatus(model);
                if (Id > 0)
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
                return PartialView(model);
            }


        }

        [HttpPost]
        [HasPermission(Permissions.Delete_TicketStatus)]
        public async Task<IActionResult> DeleteTicketStatus(int Idx)
        {
            try
            {
                var status = await _definitionService.GetTicketStatu(Idx);
                status.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                status.UpdateDate = DateTime.Now;
                status.Status = (int)GeneralStatus.Passsive;
                int labelIdx = await _definitionService.EditTicketStatus(status);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }

        }
        #endregion
        #region WorkingTypes
        public IActionResult WorkingTypes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _WorkingTypesCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _definitionService.GetWorkingTypes(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_WorkingTypes)]
        public async Task<IActionResult> AddWorkingTypes()
        {
            WorkingTypesModel workingTypesModel = new WorkingTypesModel();
            return PartialView(workingTypesModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Create_WorkingTypes)]
        public async Task<IActionResult> AddWorkingTypes(WorkingTypesModel model)
        {
            if (ModelState.IsValid)
            {
                model.InsertUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int labelIdx = await _definitionService.AddWorkingType(model);
                if (labelIdx > 0)
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
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_WorkingTypes)]
        public async Task<IActionResult> EditWorkingTypes(int Id)
        {
            var model = await _definitionService.GetWorkingType(Id);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_WorkingTypes)]
        public async Task<IActionResult> EditWorkingTypes(WorkingTypesModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                int Id = await _definitionService.EditWorkingType(model);
                if (Id > 0)
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
                return PartialView(model);
            }


        }

        [HttpPost]
        [HasPermission(Permissions.Delete_WorkingTypes)]
        public async Task<IActionResult> DeleteWorkingTypes(int Idx)
        {
            try
            {
                var workingType = await _definitionService.GetWorkingType(Idx);
                workingType.UpdateUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                workingType.UpdateDate = DateTime.Now;
                workingType.Status = (int)GeneralStatus.Passsive;
                int Id = await _definitionService.EditWorkingType(workingType);
                return Json(new JsonResponse { IsSuccess = true });
            }
            catch (Exception ex)
            {

                return Json(new JsonResponse { IsSuccess = false });
            }
        }
        #endregion
    }
}