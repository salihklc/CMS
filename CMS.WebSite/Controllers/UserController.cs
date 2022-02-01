using Microsoft.AspNetCore.Mvc;
using CMS.Core.Interfaces;
using CMS.WebSite.Helpers;
using CMS.Common.AppConstants;
using CMS.WebSite.Filters;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using CMS.Common.Models.ViewModels.Users;
using System.Linq;
using System.Collections.Generic;
using CMS.Common.Models.ViewModels.Roles;
using System;
using CMS.Common.Interfaces;
using CMS.Common;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using CMS.Common.Models.CommonModels;
using System.Security.Claims;
using Ahtapot.WebSite.Helpers;

namespace CMS.WebSite.Controllers
{
    public class UserController : AuthController
    {

        IUserService _userService;
        IGeneralServices _generalServices;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UserController(IStringLocalizer<SharedResources> localizer, IUserService userService, IGeneralServices generalServices,
            IHostingEnvironment hostingEnvironment) : base(localizer)
        {
            _userService = userService;
            _generalServices = generalServices;
            _hostingEnvironment = hostingEnvironment;
        }

        // [HasPermission(Permissions.Read_User)]
        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _UsersCallback(DTParameterModel dataTableRequest)
        {
            return Json(await _userService.GetUsers(dataTableRequest));
        }
        [HttpGet]
        [HasPermission(Permissions.Create_User)]
        public async Task<IActionResult> Create()
        {
            return PartialView();
        }
        [HttpPost]
        [HasPermission(Permissions.Create_User)]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUserAsync(model);
                return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
            }
            else
            {
                return PartialView(model);
            }

        }

        public async Task<IActionResult> Edit(int Idx)
        {
            var updateUserModel = await _userService.GetUser(Idx);
            updateUserModel.SelectedRoles = updateUserModel.Roles.Select(r => r.RoleIdx.ToString()).ToArray();

            return PartialView(updateUserModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserModel updateUserModel)
        {
            if (ModelState.IsValid)
            {
                updateUserModel.Roles = new List<UserRolesViewModel>();
                if (updateUserModel.SelectedRoles.Length > 0)
                {
                    for (int i = 0; i < updateUserModel.SelectedRoles.Length; i++)
                    {
                        UserRolesViewModel role = new UserRolesViewModel
                        {
                            Idx = Convert.ToInt32(updateUserModel.SelectedRoles[i])
                        };
                        updateUserModel.Roles.Add(role);
                    }

                }

                await _userService.UpdateUserAsync(updateUserModel);

                return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
            }
            else
            {
                return PartialView(updateUserModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int Idx)
        {
            var result = await _userService.DeleteUser(Idx, true);

            if (result)
            {
                return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });
            }
            else
            {
                return Json(new JsonResponse { IsSuccess = true, Message = _sharedLocalizer["SuccessTransaction"] });

            }

        }

        public async Task<IActionResult> UploadUsersFromExcel(string path, string extraData)
        {
            try
            {
                string folderName = "Uploads\\";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, folderName);
                string fullPath = Path.Combine(filePath, path);
                FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                DataTable dt = ExcelImporter.ReadDataFromStream(fileStream);
                List<CreateUserModel> users = new List<CreateUserModel>();
                int firmIdx = 0;
                Int32.TryParse(extraData, out firmIdx);
                foreach (DataRow row in dt.Rows)
                {
                    CreateUserModel userModel = new CreateUserModel();
                    userModel.Address = row["Address"].ToString();
                    userModel.Email = row["Email"].ToString();
                    userModel.Gsm = row["Gsm"].ToString();
                    userModel.Phone = row["Phone"].ToString();
                    userModel.UserName = row["UserName"].ToString();
                    userModel.FirstName = row["FirstName"].ToString();
                    userModel.LastName = row["LastName"].ToString();
                    userModel.Password = row["Password"].ToString();
                    userModel.TaxNo = row["TaxNo"].ToString();
                    userModel.TcNo = row["TcNo"].ToString();
                    userModel.FirmIdx = firmIdx;
                    CreateUserModelValidator validator = new CreateUserModelValidator(_userService);
                    var result = validator.Validate(userModel);
                    if (result.IsValid)
                    {
                        users.Add(userModel);
                    }
                }

                int successTransactionCount = await _userService.AddUsers(users);

                return Json(new { SuccessCount = successTransactionCount });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    SuccessCount = 0,
                    Message = ex.ToString()
                });
            }
        }

        public async Task<IActionResult> DownloadPhoto(int Idx)
        {
            var fileData = await _userService.DownloadPhoto(Idx);

            return File(fileData.ByteData, "application/octet-stream", fileData.FileName);
        }

        public async Task<IActionResult> DeletePhoto()
        {
            return Json(new JsonResponse() { IsSuccess = true });
        }

        public async Task<IActionResult> DowloadExampleUserExcel()
        {
            string folderName = "ExampleFiles\\";
            string fullPath = Path.Combine(folderName, "Users.xlsx");
            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public async Task<IActionResult> ExportExcel()
        {
            var excelData = await _userService.GetExcelUsers();

            string sFileName = @"Kullanıcılar-" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".xlsx";

            MemoryStream memory = ExcelExporter.GetExcelExportMemory(excelData.ToList());

            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}