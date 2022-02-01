using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class CityRepository :  EfRepository<Cities>, ICityRepository
    {
        public CityRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Cities>> GetCitiesWithDistricts()
        {
            var cityList = await (_dbContext as CmsDbContext).Cities
                           .Include(c => c.Districts).ToListAsync();

            return cityList;

        }
    }
}
