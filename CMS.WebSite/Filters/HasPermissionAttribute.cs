using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Core.Interfaces;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Filters
{
    //public class PermissionFilter : AuthorizeFilter
    //{
    //    readonly int _permissionNo;
    //    private IUserService _userService;
    //    public PermissionFilter(int PermissionNo,IUserService userService)
    //    {
    //        _permissionNo = PermissionNo;
    //        _userService = userService;
    //    }

    //    public override async Task  OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        var user = context.HttpContext.User;
           
    //        PermissionsHelper permissionsHelper = new PermissionsHelper(_userService);
    //        if (user.Identity.IsAuthenticated)
    //        {
    //            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
    //        }

    //        var authorize =await permissionsHelper.IsUserHasPermission(context.HttpContext.User.FindFirst(ClaimTypes.Email).Value,_permissionNo);

    //        if (!authorize)
    //        {
    //            context.Result = new  StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
    //        }
    //    }
    //}

    [AttributeUsage(AttributeTargets.Method
    | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permissions permission)
           : base(permission.ToString()) { }
    }
}
