using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class LogsRepository : EfRepository<Logs>, ILogsRepository
    {
        public LogsRepository(CmsLogDbContext dbContext) : base(dbContext)
        {

        }
    }
}
