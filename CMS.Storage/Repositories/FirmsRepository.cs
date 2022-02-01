using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Firms;
using CMS.Core.Entities;
using CMS.Core.Interfaces;


namespace CMS.Storage.Repositories
{
    public class FirmsRepository : EfRepository<Firms>, IFirmsRepository
    {
        public FirmsRepository(CmsDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> AddFirmProducts(FirmProductFields firmProduct)
        {
            try
            {
                await (_dbContext as CmsDbContext).FirmProductFields.AddAsync(firmProduct);
                _dbContext.SaveChanges();
                return firmProduct.Idx;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> AddProduct(FirmProducts firmProducts)
        {
            try
            {
                await (_dbContext as CmsDbContext).FirmProducts.AddAsync(firmProducts);
                _dbContext.SaveChanges();
                return firmProducts.Idx;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> BulkInsert(List<Firms> firms)
        {
            int success = 0;
            int fail = 0;
            foreach (var firm in firms)
            {
                try
                {
                    await AddAsync(firm);
                    success++;
                }
                catch (Exception)
                {
                    fail++;
                }
            }
            return success;

        }

        public async Task<bool> DeleteFirm(int Id)
        {
            var firm = await (_dbContext as CmsDbContext).Firms
             .Where(w => w.Status == 0)
             .FirstOrDefaultAsync(k => k.Idx == Id);

            if (firm != null)
            {

                firm.UpdateDate = DateTime.Now;
                firm.Status = 1;
                (_dbContext as CmsDbContext).Update(firm);
                (_dbContext as CmsDbContext).SaveChanges();

            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<bool> FirmNameIsUnique(string firmName, int Idx)
        {
            bool isExist;
            if (Idx > 0)
            {
                isExist = (_dbContext as CmsDbContext).Firms.Any(r => r.FirmName == firmName && r.Idx != Idx);
            }
            else
            {
                isExist = (_dbContext as CmsDbContext).Firms.Any(r => r.FirmName == firmName);
            }
            return isExist;
        }

        public async Task<List<FirmProducts>> GetFirmAllProducts(int firmIdx)
        {
          return await (_dbContext as CmsDbContext).FirmProducts.Include(k => k.Firms).Include(l => l.Products).Where(m => m.FirmIdx == firmIdx).ToListAsync();
   
        }

        public async Task<Firms> GetFirmByTaxNo(string taxNo)
        {
            var firm = await (_dbContext as CmsDbContext).Firms.FirstOrDefaultAsync(r => r.TaxNo == taxNo);
            return firm;
        }

        public async Task<Firms> GetFirmByTcNo(string tcNo)
        {
            var firm = await (_dbContext as CmsDbContext).Firms.FirstOrDefaultAsync(r => r.TcNo == tcNo);
            return firm;
        }

        public async Task<FirmProductFields> GetFirmProductFields(int FirmProductIdx, int ProductFieldIdx)
        {
            var firmProductFields = await (_dbContext as CmsDbContext).FirmProductFields.FirstOrDefaultAsync(r => r.FirmProductIdx == FirmProductIdx && r.ProductFieldIdx == ProductFieldIdx);
            return firmProductFields;
        }

        public async Task<DataTableResponse<FirmProducts>> GetFirmProducts(IDataTableRequest spec, int FirmIdx)
        {

            return DataTableEvaluator<FirmProducts>.GetQueryable((_dbContext as CmsDbContext).FirmProducts.Include(k => k.Products).Include(l => l.Firms).Where(r => r.FirmIdx == FirmIdx), spec);
        }

        public async Task<FirmProducts> GetFirmProducts(int Idx)
        {
            var firmProduct = await (_dbContext as CmsDbContext).FirmProducts.Include(k => k.Firms).Include(l => l.Products).FirstOrDefaultAsync(m => m.Idx == Idx);
            return firmProduct;
        }

        public async Task<DataTableResponse<Tickets>> GetFirmTickets(IDataTableRequest spec, FirmTicketFilterModel filter)
        {
            IQueryable<Tickets> query = (_dbContext as CmsDbContext).Tickets.Include(r=> r.Firms).Include(k=> k.Priorities).Include(l=> l.TicketTypes);
            if (filter.FirmIdx.GetValueOrDefault() > 0)
            {
                query = query.Where(r => r.FirmIdx == filter.FirmIdx);
            }
            if (!string.IsNullOrEmpty(filter.StartDate))
            {
                query = query.Where(r => r.StartDate <= Convert.ToDateTime(filter.StartDate));
            }
            if (!string.IsNullOrEmpty(filter.EndDate))
            {
                query = query.Where(r => r.DueDate <= Convert.ToDateTime(filter.EndDate));
            }
            if (filter.PriorityIdx.GetValueOrDefault() > 0)
            {
                query = query.Where(r => r.PriorityIdx == filter.PriorityIdx);
            }
            if (filter.TypeIdx.GetValueOrDefault() > 0)
            {
                query = query.Where(r => r.TypeIdx == filter.TypeIdx);
            }
            if (!string.IsNullOrEmpty(filter.TicketNumber))
            {
                query = query.Where(r => r.TicketNumber == filter.TicketNumber);
            }
            return DataTableEvaluator<Tickets>.GetQueryable(query, spec);

        }

        public async Task<IQueryable<User>> GetFirmUsers(int FirmIdx)
        {
            var users = (_dbContext as CmsDbContext).Users.Where(r => r.FirmIdx == FirmIdx);
            return users;
        }

        public async Task<bool> TaxNoIsUnique(string taxNo, int Idx)
        {
            bool isExist;
            if (Idx > 0)
            {
                isExist = (_dbContext as CmsDbContext).Firms.Any(r => r.TaxNo == taxNo && r.Idx != Idx);
            }
            else
            {
                isExist = (_dbContext as CmsDbContext).Firms.Any(r => r.TaxNo == taxNo);
            }

            return isExist;
        }
    }
}
