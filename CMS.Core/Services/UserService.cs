using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Helpers;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Common.Models.ViewModels.Users;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IAsyncRepository<UserRoles> _userroleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFirmsRepository _firmRepository;
        private readonly IOptions<FoldersSettings> _FolderOptions;

        private readonly IMapper _mapper;

        // private readonly IAppLogger<BasketService> _logger;

        public UserService(
                IUserRepository userrepository,
                IMapper mapper,
                IAsyncRepository<UserRoles> userroleRepository,
                IFirmsRepository firmRepository,
                IOptions<FoldersSettings> FolderOptions,
                ILogger<UserService> logger
             )
        {
            _userRepository = userrepository;
            _mapper = mapper;
            _userroleRepository = userroleRepository;
            _firmRepository = firmRepository;
            _FolderOptions = FolderOptions;
            _logger = logger;
        }

        //Sadece user değil user'un tüm özelliklerini ekleyecek yer burası. Aynı mediatorHandler gibi.
        public async Task CreateUserAsync(CreateUserModel model)
        {
            try
            {
                User user = _mapper.Map<User>(model);
                user.UserRoles = new List<UserRoles>();
                foreach (var role in model.SelectedRoles)
                {
                    var UserRoles = new UserRoles
                    {
                        RoleIdx = Convert.ToInt32(role),
                        InsertDate = DateTime.Now,
                        InsertUserIdx = 1,

                    };
                    user.UserRoles.Add(UserRoles);

                }
                var userEnttity = await _userRepository.AddAsync(user);


                if (userEnttity.Idx > 0 && model.PictureData != null)
                {
                    var savedFilePath = await UploadProfilePhoto(model.PictureData);

                    var base64FileImage = ImageProcessHelper.CreateBase64ThumbFromImage(savedFilePath, 70, 60);

                    userEnttity.Picture = savedFilePath;
                    userEnttity.PictureThumb = base64FileImage;

                    await _userRepository.UpdateAsync(userEnttity);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Create User Exception.");
                throw ex;
            }
            //User role ekleme burada yapılacak.

        }

        public async Task UpdateUserAsync(UpdateUserModel model)
        {
            try
            {
                User foundedUser = await _userRepository.GetUserWithRolesById(model.Idx);

                string oldPassword = foundedUser.Password;

                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = oldPassword;
                }

                _mapper.Map(model, foundedUser);

                foreach (var item in foundedUser.UserRoles)
                {
                    item.InsertDate = DateTime.Now;
                    item.InsertUserIdx = 1;
                }

                await _userRepository.UpdateAsync(foundedUser);


                if (model.PictureData != null && model.PictureData.Length > 0)
                {
                    if (foundedUser.Idx > 0)
                    {
                        if (File.Exists(foundedUser.Picture))
                            File.Delete(foundedUser.Picture);

                        var savedFilePath = await UploadProfilePhoto(model.PictureData);

                        var base64FileImage = ImageProcessHelper.CreateBase64ThumbFromImage(savedFilePath, 70, 60);

                        foundedUser.Picture = savedFilePath;
                        foundedUser.PictureThumb = base64FileImage;

                        await _userRepository.UpdateAsync(foundedUser);

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update User Exception.");
                throw ex;
            }
        }

        public async Task<UpdateUserModel> GetUser(int Idx)
        {
            User user = await _userRepository.GetUserWithRolesById(Idx);

            var userViewModel = _mapper.Map<UpdateUserModel>(user);

            userViewModel.Roles = new List<Common.Models.ViewModels.Roles.UserRolesViewModel>();

            foreach (var item in user.UserRoles)
            {
                userViewModel.Roles.Add(new Common.Models.ViewModels.Roles.UserRolesViewModel()
                {
                    Idx = item.Idx,
                    RoleIdx = item.RoleIdx
                });
            }

            return userViewModel;
        }

        public async Task<DataTableResponse<UserViewModel>> GetUsers(IDataTableRequest request)
        {
            //User entity ile UserViewModel arası dönüşümü burada yapıcaz.
            var userEntity = await _userRepository.ListAsync(request);

            DataTableResponse<UserViewModel> returnModel = new DataTableResponse<UserViewModel>();
            returnModel.Draw = userEntity.Draw;
            returnModel.RecordsFiltered = userEntity.RecordsFiltered;
            returnModel.RecordsTotal = userEntity.RecordsTotal;

            returnModel.Data = userEntity.Data.Select(f =>
            new UserViewModel()
            {
                Idx = f.Idx,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Email = f.Email
            }).ToList();

            return returnModel;
        }
        public async Task<List<UserViewModel>> GetUsers()
        {
            //User entity ile UserViewModel arası dönüşümü burada yapıcaz.
            var userEntity = await _userRepository.ListAllAsync();
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var uEntity in userEntity)
            {
                UserViewModel userViewModel = new UserViewModel
                {
                    Address= uEntity.Address,
                    CityNo = uEntity.CityNo,
                    DateOfBirth = uEntity.DateOfBirth,
                    DefaultLanguage = uEntity.DefaultLanguage,
                    DepartmanIdx = uEntity.DepartmanIdx,
                    DistrictNo =uEntity.DistrictNo,
                    Email = uEntity.Email,
                    FirmIdx = uEntity.FirmIdx,
                    FirstName = uEntity.FirstName,
                    LastName = uEntity.LastName,
                    Gsm = uEntity.Gsm,
                    UserName =uEntity.UserName
                };
                users.Add(userViewModel);
            }
            return users;
        }

        public async Task<bool> IsUserHasPermission(string email, int permissionNo)
        {
            return await _userRepository.IsUserHasPermission(email, permissionNo);
        }

        public async Task<UserModel> GetUser(string Email, string Password)
        {
            User user = await _userRepository.GetUserWithRoles(Email, Password);

            var userViewModel = new UserModel();

            userViewModel = user.xCopyTo<UserModel>();
            userViewModel.RolePermissions = new List<Permission>();
            if (user != null && user.UserRoles != null)
            {
                foreach (var uRole in user.UserRoles)
                {
                    foreach (var permissions in uRole.Roles.RolePermissions)
                    {
                        if (!userViewModel.RolePermissions.Any(r => r.PermissionNo == permissions.PermissionNo))
                        {
                            userViewModel.RolePermissions.Add(permissions.Permissions.xCopyTo<Permission>());
                        }

                    }
                }
            }

            return userViewModel;
        }

        public async Task<int> AddUsers(List<CreateUserModel> createUserModels)
        {
            int successTransaction = 0;
            foreach (var userModel in createUserModels)
            {
                try
                {
                    int firmIdx = userModel.FirmIdx ?? 0;
                    if (firmIdx == 0 && !string.IsNullOrEmpty(userModel.TaxNo))
                    {
                        var firm = await _firmRepository.GetFirmByTaxNo(userModel.TaxNo);
                        firmIdx = firm != null ? firm.Idx : 0;
                    }
                    if (firmIdx == 0 && !string.IsNullOrEmpty(userModel.TcNo))
                    {
                        var firm = await _firmRepository.GetFirmByTcNo(userModel.TcNo);
                        firmIdx = firm != null ? firm.Idx : 0;
                    }
                    userModel.FirmIdx = firmIdx;
                    await CreateUserAsync(userModel);
                    successTransaction++;
                }
                catch (Exception)
                {

                }
            }
            return successTransaction;
        }

        private async Task<string> UploadProfilePhoto(IFormFile Attachment)
        {
            string filePath = _FolderOptions.Value.Uploads;
            string fullPath = "";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (Attachment.Length > 0)
            {
                string fileName = Attachment.FileName;
                string nfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                fullPath = Path.Combine(filePath, nfileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    Attachment.CopyTo(stream);
                }
            }

            return fullPath;
        }

        public async Task<Attachment> DownloadPhoto(int key)
        {
            Attachment result = new Attachment();

            var attchmentData = await _userRepository.GetByIdAsync(key);

            result.FileName = attchmentData.Picture.Split('/').Last();

            using (FileStream SourceStream = File.Open(attchmentData.Picture, FileMode.Open))
            {
                result.ByteData = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result.ByteData, 0, (int)SourceStream.Length);
            }

            return result;
        }
        public async Task<bool> EmailIsUnique(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || user.Status == (int)GeneralStatus.Passsive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int userId, bool softDelete)
        {
            return await _userRepository.DeleteUserById(userId, softDelete);
        }

        public async Task<List<ExcelUserModel>> GetExcelUsers()
        {
            //User entity ile UserViewModel arası dönüşümü burada yapıcaz.
            var userEntity = await _userRepository.GetExcelUsers();
            List<ExcelUserModel> users = new List<ExcelUserModel>();
            foreach (var uEntity in userEntity)
            {
                ExcelUserModel userViewModel = new ExcelUserModel
                {
                    Address = uEntity.Address,
                    CityNo = uEntity.CityNo,
                    DateOfBirth = uEntity.DateOfBirth,
                    DefaultLanguage = uEntity.DefaultLanguage,
                    DistrictNo = uEntity.DistrictNo,
                    Email = uEntity.Email,
                    FirstName = uEntity.FirstName,
                    LastName = uEntity.LastName,
                    Gsm = uEntity.Gsm,
                    UserName = uEntity.UserName,
                    Firm= uEntity.Firms == null ? "": uEntity.Firms.FirmName,
                    FirmIdx = uEntity.FirmIdx
                };
                users.Add(userViewModel);
            }
            return users;
        }
    }
}