using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Accounts;
using CMS.WebSite.Helpers;

namespace CMS.WebSite.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(LoginUserModel loginUserModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginUserModel);
            }

            var user = await _userService.GetUser(loginUserModel.Email, loginUserModel.Password);

            if (user == null || user.Idx == 0)
            {
                ModelState.AddModelError(string.Empty, "Giriş Bilgileri Geçersizdir.");
                return View(loginUserModel);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Idx.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Name, user.FirstName??""),
                    new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                    new Claim(ClaimTypes.GivenName, user.UserName ?? ""),
                    //new Claim(ClaimTypes.Thumbprint, user.PictureThumb ?? ""),
                    new Claim("AuthDate",DateTime.Now.ToString()),
                    new Claim(ClaimTypes.MobilePhone,user.Phone ?? "")
                };

                HttpContext.Session.Set("PictureThumb", user.PictureThumb);
                HttpContext.Session.Set("Permissions", JsonConvert.SerializeObject(user.RolePermissions));

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                #region ek bilgiler

                var authProperties = new AuthenticationProperties
                {

                    IsPersistent = true,
                };
                #endregion
                try
                {
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                    //PermissionsHelper.SetUserPermissios(user.RolePermissions, user.Idx);
                    var url = Url.Action("Index", "Home");
                    return Redirect(url);
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }


        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(Url.Action("Index", "Account"));
        }

    }
}