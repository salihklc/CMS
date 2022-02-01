using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionsRepository _permissionRepository;
        IMapper _mapper;
        public PermissionService(IPermissionsRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public async Task<List<Permission>> GetAllPermissions()
        {
            var permissions = await _permissionRepository.GetPermissionsWithPage();
            List<Permission> allPermission = new List<Permission>();
            foreach (var permission in permissions)
            {
             
                allPermission.Add(permission.xCopyTo<Permission>());
            }
            return allPermission;

        }
    }
}
