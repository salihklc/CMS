using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class PermissionsRepository : EfRepository<Permissions>, IPermissionsRepository
    {
        public PermissionsRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Permissions>> GetPermissionsWithPage()
        {
            var permissions = await (_dbContext as CmsDbContext).Permissions
                   .Include(r => r.Page).ToListAsync();

            return permissions;
        }
    }
}
