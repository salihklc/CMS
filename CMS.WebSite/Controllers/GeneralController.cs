using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.General;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.Core.Interfaces;

namespace CMS.WebSite.Controllers
{
    public class GeneralController : AuthController
    {
        private IGeneralServices _generalServices;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITicketService _ticketService;
        public GeneralController(IStringLocalizer<SharedResources> localizer, IGeneralServices generalServices, IHostingEnvironment hostingEnvironment,
           ITicketService ticketService) : base(localizer)
        {
            _generalServices = generalServices;
            _hostingEnvironment = hostingEnvironment;
            _ticketService = ticketService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetCities()
        {
            var mCityList = await _generalServices.GetCities();

            return Json(mCityList);
        }

        public async Task<ActionResult> GetDistricts(int cityNo)
        {
            var mdistrictList = await _generalServices.GetDistricts(cityNo);

            return Json(mdistrictList);
        }
        public async Task<ActionResult> GetCitiesSelect()
        {
            var mCityList = await _generalServices.GetCitiesSelect();

            return Json(mCityList);
        }

        public async Task<ActionResult> GetDistrictsSelect(int cityNo)
        {
            var mdistrictList = await _generalServices.GetDistrictsSelect(cityNo);

            return Json(mdistrictList);
        }
        public async Task<ActionResult> GetPriorities()
        {
            var prioryList = await _generalServices.GetPriorities();
            return Json(prioryList);
        }
        public async Task<ActionResult> GetTicketTypes()
        {
            var ticketTypes = await _generalServices.GetTicketTypes();
            return Json(ticketTypes);
        }

        public async Task<ActionResult> GetWorkingTypes()
        {
            var workingTypes = await _generalServices.GetWorkingTypes();
            return Json(workingTypes);
        }

        public async Task<IActionResult> GetTicketUsers()
        {
            var users = await _generalServices.GetTicketUsers();
            return Json(users);
        }
        public async Task<IActionResult> GetTicketFirmUsers(int firmIdx)
        {
            var users = await _generalServices.GetTicketUsers(firmIdx);
            return Json(users);
        }

        public async Task<IActionResult> GetTicketStatus()
        {
            var ticketStatus = await _generalServices.GetTicketStatus();
            return Json(ticketStatus);
        }

        public async Task<IActionResult> GetTicketStatusCategories()
        {
            var ticketStatus = await _generalServices.GetTicketStatusCategories();
            return Json(ticketStatus);
        }
        public async Task<IActionResult> GetFirms()
        {
            var firms = await _generalServices.GetFirms();
            return Json(firms);
        }
        [HttpPost]
        public IActionResult ExcelImport(ExcelImportModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult ExcelFileImport(IFormFile ExcelFile, ExcelImportModel model)
        {
            string folderName = "Uploads\\";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, folderName);
            string nfileName = Guid.NewGuid().ToString() + Path.GetExtension(ExcelFile.FileName);
            string fullPath = Helpers.ExcelImporter.SaveFile(ExcelFile, filePath, nfileName);
            FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            DataTable datatable = Helpers.ExcelImporter.ReadDataFromStream(fs);

            GenericDataTable genericDataTable = new GenericDataTable();
            List<string> columns = new List<string>();

            foreach (DataColumn column in datatable.Columns)
            {
                columns.Add(column.ColumnName);
            }
            genericDataTable.Colums = JsonConvert.SerializeObject(columns);
            List<List<string>> rows = new List<List<string>>();


            foreach (DataRow row in datatable.Rows)
            {
                List<string> rowdata = new List<string>();
                foreach (string column in columns)
                {
                    rowdata.Add(row[column].ToString());
                }
                rows.Add(rowdata);
            }
            genericDataTable.Rows = JsonConvert.SerializeObject(rows);
            model.UploadExcelPath = nfileName;
            genericDataTable.ExcelImportModel = model;
            return PartialView(genericDataTable);
        }

        public async Task<IActionResult> GetRelatedTickets(int TicketIdx)
        {
            var ticket = await _ticketService.GetTickets(TicketIdx);

            var res = await _generalServices.GetRelatedTickets(ticket.FirmIdx);

            return Json(res);
        }
        public async Task<IActionResult> GetFieldTypes()
        {
            var fields = await _generalServices.GetFieldTypes();
            return Json(fields);
        }

        public async Task<IActionResult> GetFields()
        {
            var fields = await _generalServices.GetFields();
            return Json(fields);
        }
        public async Task<IActionResult> GetFieldsAllData(int productIdx = 0)
        {
            var fields = await _generalServices.GetFieldsAllData(productIdx);
            return Json(fields);
        }

        public async Task<IActionResult> GetSelectedFieldsAllData(int productIdx = 0)
        {
            var fields = await _generalServices.GetSelectedFieldsAllData(productIdx);
            return Json(fields);
        }

        public async Task<IActionResult> GetProducts()
        {
            var products = await _generalServices.GetProducts();
            return Json(products);
        }
        public async Task<IActionResult> GetProductFields(int ProductIdx)
        {
            var productFields = await _generalServices.GetProductFields(ProductIdx);
            return Json(productFields);
        }
        public async Task<IActionResult> GetFirmProductFields(int FirmProductIdx)
        {
            var productFields = await _generalServices.GetFirmProductFields(FirmProductIdx);
            return Json(productFields);
        }

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _generalServices.GetRoles();
            return Json(roles);
        }

        public async Task<IActionResult> GetTicketsCount()
        {
            int AssiggneeUserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var countOfTickets = await _ticketService.GetAllTicketsCounts(AssiggneeUserIdx);
            
            return Json(countOfTickets);
        }

        public async Task<IActionResult> GetHeadersData()
        {

            return Json("");
        }
    }
}