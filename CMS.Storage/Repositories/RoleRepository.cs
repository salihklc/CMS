using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class RoleRepository : EfRepository<Roles>, IRolesRepository
    {
        public RoleRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Roles>> GetRoleWithPermissions(int RoleIdx)
        {
            try
            {
                var roles = await (_dbContext as CmsDbContext).Roles.Where(x => x.Idx == RoleIdx)
                       .Include(r => r.RolePermissions).ThenInclude(k=> k.Permissions).ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {

                throw;
            }
         
        }

        public async Task<bool> IsUniqueRoleName(string RoleName)
        {
            return await  (_dbContext as CmsDbContext).Roles.AnyAsync(r => r.RoleName == RoleName);
        }
    }
}
