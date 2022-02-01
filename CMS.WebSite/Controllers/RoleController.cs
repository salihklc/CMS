using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CMS.Common;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Roles;
using CMS.WebSite.Filters;

namespace CMS.WebSite.Controllers
{
    public class RoleController : AuthController
    {
        IRoleService _roleService;
        IPermissionService _permissionService;
        public RoleController(IStringLocalizer<SharedResources> localizer,IRoleService roleService, IPermissionService permissionService) : base(localizer)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }
      
        public async Task<IActionResult> Index()
        {
            RolesWithPermissions rolesWithPermissions = new RolesWithPermissions();
            var roles = await  _roleService.GetAllRoles();
            var permissions = await _permissionService.GetAllPermissions();
            rolesWithPermissions.RoleViewModels = roles;
            rolesWithPermissions.RolePermissions = permissions;
            return View(rolesWithPermissions);
        }

        public async Task<IActionResult> GetRolePermissions(int Id)
        {
            var rolePermissions =await _roleService.GetRoleWithPermissions(Id);
            return Json(rolePermissions);
        }
        public IActionResult AddRole()
        {
            CreateRoleModel model = new CreateRoleModel();
            return PartialView(model);
        }
        [HttpPost]
        [HasPermission(Permissions.Create_Role)]
        public async Task<IActionResult> AddRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var roleId = await _roleService.AddRole(model);
                var redirectUrl = Url.Action("Index", "Role");
                return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır.", ReturnUrl = redirectUrl });
            }
            else
            {
                return PartialView(model);
            }
         
        }
        [HttpPost]
        [HasPermission(Permissions.Edit_Role)]
        public async Task<IActionResult> AddRolePermission(RolesWithPermissions rolesWithPermissions)
        {
            var id = await _roleService.AddRolePermissions(rolesWithPermissions);
            var redirectUrl = Url.Action("Index", "Role");
            return Json(new JsonResponse { IsSuccess = true, Message = "İşlem Başarılıdır.", ReturnUrl = redirectUrl });
        }
    }
}