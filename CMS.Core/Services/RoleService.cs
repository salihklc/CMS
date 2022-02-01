using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{

    public class RoleService : IRoleService
    {

        private readonly IRolesRepository _rolesRepository;
        private readonly IAsyncRepository<RolePermissions> _rolepermissionsRepository;
        IMapper _mapper;


        public RoleService(IRolesRepository rolesRepository, IMapper mapper, IAsyncRepository<RolePermissions> rolepermissionsRepository)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
            _rolepermissionsRepository = rolepermissionsRepository;
        }

        public async Task<List<RoleViewModel>> GetAllRoles()
        {
            var roles = await _rolesRepository.ListAllAsync();

            return _mapper.Map<List<RoleViewModel>>(roles);
        }



        public async Task<List<RolePermissionViewModel>> GetRoleWithPermissions(int RoleIdx)
        {
            try
            {
                var roles = await _rolesRepository.GetRoleWithPermissions(RoleIdx);
                List<RolePermissionViewModel> roleList = new List<RolePermissionViewModel>();
                foreach (var role in roles)
                {
                    var rolevw = role.xCopyTo<RolePermissionViewModel>();
                    foreach (var permissions in role.RolePermissions)
                    {
                        var permissionvw = permissions.xCopyTo<RolePermission>();
                        permissionvw.Permission = permissions.Permissions.xCopyTo<Permission>();
                        rolevw.RolePermissions.Add(permissionvw);
                    }
                    roleList.Add(rolevw);
                }

                return roleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<int> AddRole(CreateRoleModel model)
        {
            try
            {
                var role = model.xCopyTo<Roles>();
                var res = await _rolesRepository.AddAsync(role);
                return res.Idx;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> AddRolePermissions(RolesWithPermissions rolePermissions)
        {
            try
            {
                var Role = (await _rolesRepository.GetRoleWithPermissions(rolePermissions.RoleIdx)).FirstOrDefault();
                List<RolePermissions> permissions = new List<RolePermissions>();
                permissions = Role.RolePermissions.ToList();
                // delete all permissions
                foreach (var perm in permissions)
                {
                     await _rolepermissionsRepository.DeleteAsync(perm);
                }
               

                Role.RolePermissions = new List<RolePermissions>();
                foreach (var per in rolePermissions.RolePermissions)
                {
                    Role.RolePermissions.Add(new RolePermissions
                    {
                        PermissionNo = per.PermissionNo,
                        InsertDate = DateTime.Now,
                        InsertUserIdx = 1,
                        UpdateDate = DateTime.Now,
                        UpdateUserIdx = 1,
                        RoleIdx = rolePermissions.RoleIdx
                    });
                }
                await _rolesRepository.UpdateAsync(Role);
                return 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> IsUniqueRoleName(string RoleName)
        {
            return await _rolesRepository.IsUniqueRoleName(RoleName);
        }
    }
}