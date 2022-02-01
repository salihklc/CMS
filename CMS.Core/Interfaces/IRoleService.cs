using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IRoleService
    {
        Task<List<RolePermissionViewModel>> GetRoleWithPermissions(int RoleIdx);

        Task<List<RoleViewModel>> GetAllRoles();
        Task<int> AddRole(CreateRoleModel createRoleModel);
        Task<int> AddRolePermissions(RolesWithPermissions rolePermissions);
    }
}
