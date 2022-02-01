using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IRolesRepository : IAsyncRepository<Roles>
    {
        Task<List<Roles>> GetRoleWithPermissions(int RoleIdx);
        Task<bool> IsUniqueRoleName(string RoleName);
    }
}
