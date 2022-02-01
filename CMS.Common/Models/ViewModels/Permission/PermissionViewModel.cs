using System.Collections.Generic;

namespace CMS.Common.Models.ViewModels.Permission
{
    public class RolePermissionViewModel
    {
        public int Idx { get; set; }
        public string RoleName { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public RolePermissionViewModel()
        {
            RolePermissions = new List<RolePermission>();
        }

    }
    public class RolePermission
    {
        public int Idx { get; set; }
        public int PermissionNo { get; set; }
    
        public Permission Permission { get; set; }
        public RolePermission()
        {
            Permission = new Permission();
        }
    }

    public class Permission
    {
        public int PermissionNo { get; set; }
        public string PermissionName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public Page Page { get; set; }
        public Permission()
        {
            Page = new Page();
        }
    }

    public class Page
    {
        public int Idx { get; set; }
        public string PageName { get; set; }
    }
}