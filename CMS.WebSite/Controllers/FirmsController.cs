using Ahtapot.WebSite.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Firms;
using CMS.WebSite.Filters;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Controllers
{
    public class FirmsController : BaseController
    {
        private readonly IFirmService _firmService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public FirmsController(IStringLocalizer<SharedResources> localizer, IFirmService firmService, IHostingEnvironment hostingEnvironment) : base(localizer)
        {
            _firmService = firmService;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HasPermission(Permissions.Read_Firms)]
        [HttpPost]
        public async Task<IActionResult> _FirmsCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _firmService.GetFirms(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_Firms)]
        public async Task<IActionResult> AddFirm()
        {
            FirmsModel model = new FirmsModel();
            model.InsertUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Create_Firms)]
        public async Task<IActionResult> AddFirm(FirmsModel model)
        {
            if (ModelState.IsValid)
            {
                int firmId = await _firmService.AddFirm(model);
                if (firmId > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _sharedLocalizer["FailureTransaction"] });
                }
            }
            else
            {
                return PartialView(model);
            }
        }

        [HttpGet]
        [HasPermission(Permissions.Edit_Firms)]
        public async Task<IActionResult> EditFirm(int Id)
        {
            FirmsModel model = await _firmService.GetFirm(Id);
            model.UpdateUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_Firms)]
        public async Task<IActionResult> EditFirm(FirmsModel model)
        {
            if (ModelState.IsValid)
            {
                int firmId = await _firmService.EditFirm(model);
                if (firmId > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _sharedLocalizer["FailureTransaction"] });
                }
            }
            else
            {
                return PartialView(model);
            }
        }
        [HttpPost]
        [HasPermission(Permissions.Delete_Firms)]
        public async Task<IActionResult> Delete(int Idx)
        {
            var res = await _firmService.DeleteFirm(Idx);
            if (res)
            {
                return Json(new JsonResponse { IsSuccess = res, Message = _sharedLocalizer["SuccessTransaction"] });
            }
            return Json(new JsonResponse { IsSuccess = res, Message = _sharedLocalizer["FailureTransaction"] });
        }

        public async Task<IActionResult> DowloadExampleFirmExcel()
        {
            string folderName = "ExampleFiles\\";
            string fullPath = Path.Combine(folderName, "Firms.xlsx");
            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> UploadFirmsFromExcel(string path)
        {
            try
            {
                string folderName = "Uploads\\";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, folderName);
                string fullPath = Path.Combine(filePath, path);
                FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                DataTable dt = ExcelImporter.ReadDataFromStream(fileStream);
                List<FirmsModel> firms = new List<FirmsModel>();
                foreach (DataRow row in dt.Rows)
                {
                    FirmsModel firmsModel = new FirmsModel();
                    firmsModel.Address = row["Address"].ToString();
                    firmsModel.Email = row["Email"].ToString();
                    firmsModel.FirmName = row["FirmName"].ToString();
                    firmsModel.Gsm = row["Gsm"].ToString();
                    firmsModel.Gsm2 = row["Gsm2"].ToString();
                    firmsModel.CommercialTitle = row["CommercialTitle"].ToString();
                    firmsModel.ContactName = row["ContactName"].ToString();
                    firmsModel.ContactSurname = row["ContactSurname"].ToString();
                    firmsModel.Phone = row["Phone"].ToString();
                    firmsModel.Phone2 = row["Phone2"].ToString();
                    firmsModel.TaxNo = row["TaxNo"].ToString();
                    firmsModel.TcNo = row["TcNo"].ToString();

                    FirmsModelValidator validator = new FirmsModelValidator(_firmService);
                    var result = validator.Validate(firmsModel);
                    if (result.IsValid)
                    {
                        firmsModel.InsertDate = DateTime.Now;
                        firmsModel.InsertUserIdx = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                        firms.Add(firmsModel);
                    }
                }

                int successTransactionCount = await _firmService.AddFirms(firms);

                return Json(new { SuccessCount = successTransactionCount });
            }
            catch (Exception ex)
            {

                return Json(new { SuccessCount = 0, Message = ex.ToString() });
            }



        }

        [HasPermission(Permissions.Read_FirmProducts)]
        public async Task<IActionResult> FirmProducts(int FirmIdx)
        {

            return View(FirmIdx);
        }

        [HasPermission(Permissions.Read_FirmProducts)]
        [HttpPost]
        public async Task<IActionResult> _FirmProductsCallBack(DTParameterModel dataTableRequest, int FirmIdx)
        {
            return Json(await _firmService.GetFirmProducts(dataTableRequest, FirmIdx));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_FirmProducts)]
        public async Task<IActionResult> AddFirmProduct(int FirmIdx)
        {
            FirmProductFieldsModel model = new FirmProductFieldsModel();
            if (FirmIdx > 0)
            {
                FirmsModel firmModel = await _firmService.GetFirm(FirmIdx);

                model.FirmIdx = FirmIdx;
                model.FirmName = firmModel.FirmName;

            }
            else
            {
                model.FirmIdx = 0;
            }

            return View(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Create_FirmProducts)]
        public async Task<IActionResult> AddFirmProduct(FirmProductFieldsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _firmService.AddFirmProducts(model);
                    return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
                }
                catch (Exception ex)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = ex.Message.ToString() });
                }

            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Edit_FirmProducts)]
        public async Task<IActionResult> EditFirmProduct(int FirmProductIdx)
        {

            FirmProductFieldsModel model = new FirmProductFieldsModel();
            model.FirmProductIdx = FirmProductIdx;
            var firmProduct = await _firmService.GetFirmProductsModels(FirmProductIdx);
            model.FirmName = firmProduct.FirmName;
            model.ProductIdx = firmProduct.ProductIdx;
            model.ProductName = firmProduct.ProductName_TR;
            model.FirmIdx = firmProduct.FirmIdx;
            model.UserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View(model);
        }

        [HttpPost]
        [HasPermission(Permissions.Edit_FirmProducts)]
        public async Task<IActionResult> EditFirmProduct(FirmProductFieldsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _firmService.EditFirmProducts(model);
                    return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
                }
                catch (Exception ex)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = ex.Message.ToString() });
                }

            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Products(int FirmIdx)
        {
            return PartialView(FirmIdx);
        }

        [HasPermission(Permissions.Read_FirmProducts)]
        public async Task<IActionResult> FirmTickets(int FirmIdx)
        {

            return View(FirmIdx);
        }
        [HttpPost]
        public async Task<IActionResult> Tickets(FirmTicketFilterModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> _FirmTicketsCallBack(DTParameterModel dataTableRequest, int FirmIdx,string StartDate, string EndDate, int PriorityIdx, int TypeIdx, string TicketNumber)
        {
            FirmTicketFilterModel filter = new FirmTicketFilterModel
            {
                FirmIdx = FirmIdx,
                EndDate = EndDate,
                StartDate = StartDate,
                PriorityIdx = PriorityIdx,
                TicketNumber = TicketNumber,
                TypeIdx = TypeIdx
            };
            return Json(await _firmService.GetFirmTickets(dataTableRequest, filter));
        }
        public async Task<IActionResult> ExportExcel()
        {
            var excelData = await _firmService.GetFirms();

            string sFileName = @"Firmalar-" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".xlsx";

            MemoryStream memory = ExcelExporter.GetExcelExportMemory(excelData);

            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> ExportFirmProductExcel(int firmIdx)
        {
            var excelData = await _firmService.GetFirmProducts(firmIdx);

            string sFileName = @"FirmaÜrünler-" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".xlsx";

            MemoryStream memory = ExcelExporter.GetExcelExportMemory(excelData);

            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}