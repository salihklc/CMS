using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using CMS.Common.Models.ViewModels.Users;

public class ViewBagFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            string picture = "/files/img/profile-photos/user.png";
            string permission = "";
            try
            {
                picture = context.HttpContext.Session.Get<string>("PictureThumb");
                permission = context.HttpContext.Session.Get<string>("Permissions");
            }
            catch (Exception ex)
            {


            }

            UserModel userModel = new UserModel()
            {
                Idx = System.Int32.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Email = context.HttpContext.User.FindFirst(ClaimTypes.Email).Value,
                FirstName = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value,
                LastName = context.HttpContext.User.FindFirst(ClaimTypes.Surname).Value,
                PictureThumb = picture,
                Permissions = permission
            };

            var controller = context.Controller as Controller;
            controller.ViewBag.AuthUser = userModel;

            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];

            controller.ViewData["SelectedMenu"] = string.Format(".{0}-{1}", controllerName, actionName);
            controller.ViewData["AdminTitle"] = "Yönetim Paneli";


        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // do something after the action executes
    }
}