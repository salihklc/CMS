using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface ICityRepository : IAsyncRepository<Cities>
    {
        Task<List<Cities>> GetCitiesWithDistricts();
    }
}
