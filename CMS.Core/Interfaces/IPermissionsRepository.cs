using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IPermissionsRepository : IAsyncRepository<Permissions>
    {
        Task<List<Permissions>> GetPermissionsWithPage();
    }
}
