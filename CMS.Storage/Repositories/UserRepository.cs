using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Users;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public User CheckUserAndRole(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public bool ValidateLastChanged(string LastChanged)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserHasPermission(string Email, int PermissionNo)
        {
            bool result = false;

            var userPermissions = await (_dbContext as CmsDbContext).Users
                .Include(r => r.UserRoles)
                .ThenInclude(k => k.Roles).ThenInclude(l => l.RolePermissions).FirstOrDefaultAsync(k => k.Email == Email);

            if (userPermissions == null)
            {
                return result;
            }

            result = userPermissions.UserRoles.Select(k => k.Roles.RolePermissions).Any(k => k.Any(r => r.PermissionNo == PermissionNo));

            return result;

        }

        public async Task<User> GetUserWithRolesById(int Id)
        {

            var user = await (_dbContext as CmsDbContext).Users
                .Include(r => r.UserRoles)
                .ThenInclude(k => k.Roles).ThenInclude(l => l.RolePermissions)
                .Where(w => w.Status == 0)
                .FirstOrDefaultAsync(k => k.Idx == Id);
            return user;
        }

        public async Task<bool> DeleteUserById(int Id, bool IsSoftDelete)
        {
            var user = await (_dbContext as CmsDbContext).Users
               .Include(r => r.UserRoles)
               .ThenInclude(k => k.Roles).ThenInclude(l => l.RolePermissions)
               .Where(w => w.Status == 0)
               .FirstOrDefaultAsync(k => k.Idx == Id);

            if (user != null)
            {
                if (IsSoftDelete)
                {
                    user.UpdateDate = DateTime.Now;
                    user.Status = 1;
                    (_dbContext as CmsDbContext).Update(user);
                    (_dbContext as CmsDbContext).SaveChanges();
                }
                else
                {
                    await base.DeleteAsync(user);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<User> GetUserWithRoles(string Email, string Password)
        {
            try
            {
                var user = await (_dbContext as CmsDbContext).Users
              .Include(r => r.UserRoles)
              .ThenInclude(k => k.Roles).ThenInclude(l => l.RolePermissions).ThenInclude(m => m.Permissions).FirstOrDefaultAsync(k => k.Email == Email && k.Password == Password);
                return user;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IQueryable<User>> GetUsersByFirmIdx(int FirmIdx)
        {
            var users = (_dbContext as CmsDbContext).Users.Where(r => r.FirmIdx == FirmIdx);
            return users;
        }

        public async Task<User> GetUserByEmail(string Email)
        {
            var user = await (_dbContext as CmsDbContext).Users.FirstOrDefaultAsync(r => r.Email == Email);
            return user;
        }

        public async Task<List<User>> GetExcelUsers()
        {
            return (_dbContext as CmsDbContext).Users.Include(r => r.Firms).ToList();
        }
    }
}
