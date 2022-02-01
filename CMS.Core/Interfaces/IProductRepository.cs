using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IProductRepository :  IAsyncRepository<Products>
    {
        Task<DataTableResponse<Fields>> GetFields(IDataTableRequest spec);
        Task<List<Fields>> GetFields();
        Task<Fields> GetField(int Idx);
        Task<IQueryable<Fields>> GetFieldAllData(int productIdx);
        Task<bool> IsNameUnique(string Name_TR, string Name_EN, int Idx);
        Task<Products> GetProduct(int Idx);
        Task<IQueryable<Fields>> GetSelectedFieldsAllData(int productIdx);
        Task<IQueryable<ProductFields>> GetProductFields(int productIdx);
        Task<IQueryable<FirmProductFields>> GetFirmProductFields(int firmproductIdx);
    }
}
