using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Permission;

namespace CMS.Common.Interfaces
{
    public interface IPermissionService
    {
        Task<List<Permission>> GetAllPermissions();
    }
}
