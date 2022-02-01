using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ahtapot.WebSite.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Products;
using CMS.WebSite.Filters;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IStringLocalizer<SharedResources> _localizer;
        public ProductController(IProductService productService, IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }
       public IActionResult Index()
        {
            return View();
        }
        [HasPermission(Permissions.Read_Fields)]
        public ActionResult Fields()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> _FieldsCallback(DTParameterModel dataTableRequest)
        {
            return Json(await _productService.GetFields(dataTableRequest));
        }
        [HasPermission(Permissions.Create_Fields)]
        public ActionResult AddFields()
        {
            FieldsModel model = new FieldsModel();
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Create_Fields)]
        public async Task<ActionResult> AddFields(FieldsModel model)
        {
            if (ModelState.IsValid)
            {
                var fieldIdx = await  _productService.AddField(model);
                if (fieldIdx > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _localizer["FailureTransaction"] });
                }
               
            }
            else
            {
                return PartialView(model);
            }
        }

        [HasPermission(Permissions.Edit_Fields)]
        public async Task<ActionResult> EditFields(int Idx)
        {
            try
            {
                FieldsModel model = await _productService.GetField(Idx);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_Fields)]
        public async Task<ActionResult> EditFields(FieldsModel model)
        {
            if (ModelState.IsValid)
            {
                var fieldIdx = await _productService.EditField(model);
                if (fieldIdx > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _localizer["FailureTransaction"] });
                }

            }
            else
            {
                return PartialView(model);
            }
        }
        [HttpGet]
        [HasPermission(Permissions.Read_Products)]
        public IActionResult Products()
        {
            return View();
        }
        [HasPermission(Permissions.Read_Products)]
        [HttpPost]
        public async Task<IActionResult> _ProductsCallBack(DTParameterModel dataTableRequest)
        {
            return Json(await _productService.GetProducts(dataTableRequest));
        }

        [HttpGet]
        [HasPermission(Permissions.Create_Products)]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        [HasPermission(Permissions.Create_Products)]
        public async Task<IActionResult> AddProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                var Idx = await _productService.AddProduct(model);
                if (Idx > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _localizer["FailureTransaction"] });
                }

            }
            else
            {
                return View(model);
            }
        }
        [HasPermission(Permissions.Edit_Products)]
        public async Task<IActionResult> EditProduct(int Idx)
        {
            var model =await _productService.GetProductModel(Idx);
            AddProductModel addProductModel = new AddProductModel
            {
                Idx = model.Idx,
                Description_EN = model.Description_EN,
                Description_TR = model.Description_TR,
                InsertDate = model.InsertDate,
                InsertUserIdx = model.InsertUserIdx,
                Name_TR = model.Name_TR,
                Name_EN = model.Name_EN,
                Fields = model.ProductFieldsModel.FieldsModels.Select(r=> r.Idx).ToList(),
                SelectedFields = model.ProductFieldsModel
            };
            return View(addProductModel);
        }

        [HttpPost]
        [HasPermission(Permissions.Edit_Products)]
        public async Task<IActionResult> EditProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserIdx = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var Idx = await _productService.EditProduct(model);
                if (Idx > 0)
                {
                    return Json(new JsonResponse { IsSuccess = true, Message = _localizer["SuccessTransaction"] });
                }
                else
                {
                    return Json(new JsonResponse { IsSuccess = false, Message = _localizer["FailureTransaction"] });
                }

            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> ExportExcel()
        {
            var excelData = await _productService.GetProducts();

            string sFileName = @"Ürünler-" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".xlsx";

            MemoryStream memory = ExcelExporter.GetExcelExportMemory(excelData);

            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> ExportFieldsExcel()
        {
            var excelData = await _productService.GetFields();

            string sFileName = @"ÜrünAlanları-" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".xlsx";

            MemoryStream memory = ExcelExporter.GetExcelExportMemory(excelData);

            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}