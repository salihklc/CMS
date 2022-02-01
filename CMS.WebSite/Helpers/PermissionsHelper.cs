using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Core.Interfaces;

namespace CMS.WebSite.Helpers
{
    public static  class PermissionsHelper
    {
        public static List<KeyValuePair<int, List<Permission>>> UserPermissions = null;

        public static List<Permission> GetUserPermissions(int userId)
        {
          
            if (UserPermissions == null)
            {
                UserPermissions = new List<KeyValuePair<int, List<Permission>>>();
            }
            
            return UserPermissions.Where(w => w.Key == userId).FirstOrDefault().Value;
        }

        public static bool SetUserPermissios(List<Permission> Permissions, int userId)
        {
            bool result = false;
            try
            {
                if (UserPermissions == null)
                {
                    UserPermissions = new List<KeyValuePair<int, List<Permission>>>();
                }
                else if (UserPermissions.Where(r => r.Key == userId).FirstOrDefault().Value != null && UserPermissions.Where(r => r.Key == userId).FirstOrDefault().Value.Count() > 0)
                {
                    UserPermissions.RemoveAll(r => r.Key == userId);
                    UserPermissions.Add(new KeyValuePair<int, List<Permission>>(userId, Permissions));
                }
                else
                {
                    UserPermissions.Add(new KeyValuePair<int, List<Permission>>(userId, Permissions));
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }

            return result;
           
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
        }

        public string PermissionName { get; }
    }
}
