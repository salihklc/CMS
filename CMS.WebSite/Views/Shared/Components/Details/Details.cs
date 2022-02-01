using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.Details
{
    public class Details : ViewComponent
    {
        private readonly ITicketService _ticketService;
        

        public Details(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Idx)
        {
            var model =await _ticketService.GetTicketDetails(Idx);
            var Permissions = Request.HttpContext.Session.Get<string>("Permissions");

            var UserPermissions = JsonConvert.DeserializeObject<List<Common.Models.ViewModels.Permission.Permission>>(Permissions);
            model.TicketHeader.Permissions = new List<Common.Models.ViewModels.Tickets.ButtonPermissions>();
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Comment_Ticket,
                PermissionName = "Comment_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Comment_Ticket) )
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Attach_Ticket,
                PermissionName = "Attach_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Attach_Ticket))
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.ChangeState_Ticket,
                PermissionName = "ChangeState_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.ChangeState_Ticket)) &&
                                 model.TicketHeader.TicketHeader.AssigneeUserIdx == Convert.ToInt32(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value)
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Assignee_Ticket,
                PermissionName = "Assignee_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Assignee_Ticket))
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.LogTime_Ticket,
                PermissionName = "LogTime_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.LogTime_Ticket)) &&
                                 model.TicketHeader.TicketHeader.AssigneeUserIdx == Convert.ToInt32(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value)
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Reopen_Ticket,
                PermissionName = "Reopen_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Reopen_Ticket)) &&
                                model.TicketHeader.TicketHeader.InsertUserIdx == Convert.ToInt32(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value) &&
                                model.TicketHeader.TicketHeader.Status == (int)Common.AppConstants.ITicketStatus.DONE
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.ChangeDescription_Ticket,
                PermissionName = "ChangeDescription_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.ChangeDescription_Ticket)) &&
                                model.TicketHeader.TicketHeader.InsertUserIdx == Convert.ToInt32(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value) &&
                                model.TicketHeader.TicketHeader.Status != (int)Common.AppConstants.ITicketStatus.DONE &&
                                model.TicketHeader.TicketHeader.AssigneeUserIdx == 0
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.ChangeLabel_Ticket,
                PermissionName = "ChangeLabel_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.ChangeLabel_Ticket))
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Change_TicketInfos,
                PermissionName = "ChangeLabel_Ticket",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Change_TicketInfos))
            });
            model.TicketHeader.Permissions.Add(new Common.Models.ViewModels.Tickets.ButtonPermissions
            {
                PermissionNo = (int)Common.AppConstants.Permissions.Create_TicketWatchers,
                PermissionName = "Create_TicketWatchers",
                AuthorizeUser = (UserPermissions.Any(r => r.PermissionNo == (int)Common.AppConstants.Permissions.Create_TicketWatchers))
            });
            return View(model);
        }

    }
}
