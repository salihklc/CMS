using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Repositories
{
    public class PrioritiesRepository : EfRepository<Priorities>
    {
        public PrioritiesRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }   
    }
}
