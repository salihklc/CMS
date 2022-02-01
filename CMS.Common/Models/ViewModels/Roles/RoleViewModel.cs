using System.Collections.Generic;
using CMS.Common.Models.ViewModels.Permission;

namespace CMS.Common.Models.ViewModels.Roles
{
    public class RoleViewModel
    {
        public int Idx { get; set; }
        public string RoleName { get; set; }
      
    }

    public class UserRolesViewModel
    {
        public int Idx { get; set; }
        public int RoleIdx { get; set; }
        public RoleViewModel Roles { get; set; }
    }

    public class RolesWithPermissions
    {
        public List<RoleViewModel> RoleViewModels { get; set; }
        public List<Permission.Permission> RolePermissions { get; set; }
        public int RoleIdx { get; set; }
    }
}