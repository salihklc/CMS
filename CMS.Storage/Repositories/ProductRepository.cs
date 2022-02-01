using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Interfaces;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Storage.Repositories
{
    public class ProductRepository : EfRepository<Products>, IProductRepository
    {
        public ProductRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Fields> GetField(int Idx)
        {
            return await (_dbContext as CmsDbContext).Fields.FirstOrDefaultAsync(r => r.Idx == Idx);
        }

        public async Task<DataTableResponse<Fields>> GetFields(IDataTableRequest spec)
        {
            return DataTableEvaluator<Fields>.GetQueryable((_dbContext as CmsDbContext).Fields.Include(k => k.Types) , spec);     
        }
        public async Task<IQueryable<Fields>> GetFieldAllData(int productIdx)
        {
            var fields = (_dbContext as CmsDbContext).Fields.Include(k => k.Types).Where(r=> r.Status ==(int)GeneralStatus.Active);
            if (productIdx > 0)
            {
              var productFields =   (_dbContext as CmsDbContext).ProductFields.Where(r => r.ProductIdx == productIdx);
                fields = fields.Where(r => !productFields.Select(k => k.FieldIdx).Contains(r.Idx));
            }
            return fields;
        }
        public async Task<bool> IsNameUnique(string Name_TR, string Name_EN, int Idx)
        {
            return (_dbContext as CmsDbContext).Products.Any(r => (r.Name_EN == Name_EN || r.Name_TR == Name_TR) && r.Idx != Idx);

        }
        public async Task<Products> GetProduct(int Idx)
        {
            return await (_dbContext as CmsDbContext).Products.Include(r => r.ProductFields).ThenInclude(k => k.Fields).ThenInclude(l => l.Types)
                .FirstOrDefaultAsync(m => m.Idx == Idx && m.ProductFields.Any(k=>k.Status == (int)GeneralStatus.Active));
        }

        public async Task<IQueryable<Fields>> GetSelectedFieldsAllData(int productIdx)
        {
            var fields = (_dbContext as CmsDbContext).Fields.Include(k => k.Types).Where(r => r.Status == (int)GeneralStatus.Active);
            if (productIdx > 0)
            {
                var productFields = (_dbContext as CmsDbContext).ProductFields.Where(r => r.ProductIdx == productIdx);
                fields = fields.Where(r => productFields.Select(k => k.FieldIdx).Contains(r.Idx));
            }
            return fields;
        }

        public async Task<IQueryable<ProductFields>> GetProductFields(int productIdx)
        {
            return (_dbContext as CmsDbContext).ProductFields.Include(m=> m.Fields).Where(k => k.ProductIdx == productIdx);

        }

        public async Task<IQueryable<FirmProductFields>> GetFirmProductFields(int firmproductIdx)
        {
            return (_dbContext as CmsDbContext).FirmProductFields.Where(r => r.FirmProductIdx == firmproductIdx);
        }

        public async Task<List<Fields>> GetFields()
        {
            return await (_dbContext as CmsDbContext).Fields.Include(k => k.Types).ToListAsync();
        }
    }
}
