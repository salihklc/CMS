using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Users;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        User CheckUserAndRole(string Email,string Password);

        bool ValidateLastChanged(string LastChanged);

        Task<bool> IsUserHasPermission(string Email, int PermissionNo);
        Task<User> GetUserWithRolesById(int Id);
        Task<User> GetUserWithRoles(string Email, string Password);
        Task<IQueryable<User>> GetUsersByFirmIdx(int FirmIdx);
        Task<User> GetUserByEmail(string Email);
        Task<bool> DeleteUserById(int Id, bool IsSoftDelete);
        Task<List<User>> GetExcelUsers();
    }
}