using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserModel createUserModel);
        Task<bool> IsUserHasPermission(string email, int permissionNo);

        Task<DataTableResponse<UserViewModel>> GetUsers(IDataTableRequest dataTableRequest);
        Task<List<UserViewModel>> GetUsers();
        Task<List<ExcelUserModel>> GetExcelUsers();

        Task<UpdateUserModel> GetUser(int Idx);
        Task UpdateUserAsync(UpdateUserModel updateUserModel);
        Task<UserModel> GetUser(string Email, string Password);
        Task<int> AddUsers(List<CreateUserModel> createUserModels);
        Task<bool> EmailIsUnique(string email);
        Task<Attachment> DownloadPhoto(int key);
        Task<bool> DeleteUser(int userId, bool softDelete);
    }
}
