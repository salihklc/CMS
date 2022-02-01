using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Core.Interfaces;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Middleware
{
    public class PermissionAuthorizeHandler : AuthorizationHandler<PermissionRequirement>
    {
        private IUserService _userService;

        public PermissionAuthorizeHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            var user = context.User;
          
            if (user == null)
            {
                return;
            }
            if (!user.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }
            int permissionno = (int)Enum.Parse(typeof(Permissions), requirement.PermissionName);
            var email = context.User.FindFirst(ClaimTypes.Email).Value;
            var authorize = await _userService.IsUserHasPermission(email, permissionno);

            if (!authorize)
            {
                context.Fail();
                return;
            }
            else
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
