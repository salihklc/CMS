using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Firms;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces
{
    public interface IFirmsRepository : IAsyncRepository<Firms>
    {
        Task<IQueryable<User>> GetFirmUsers(int FirmIdx);
        Task<bool> TaxNoIsUnique(string taxNo, int Idx);
        Task<bool> FirmNameIsUnique(string firmName, int Idx);
        Task<int> BulkInsert(List<Firms> firms);
        Task<Firms> GetFirmByTaxNo(string taxNo);
        Task<Firms> GetFirmByTcNo(string tcNo);
        Task<DataTableResponse<FirmProducts>> GetFirmProducts(IDataTableRequest spec, int FirmIdx);
        Task<int> AddFirmProducts(FirmProductFields firmProduct);
        Task<int> AddProduct(FirmProducts firmProducts);
        Task<FirmProducts> GetFirmProducts(int Idx);
        Task<List<FirmProducts>> GetFirmAllProducts(int firmIdx);
        Task<FirmProductFields> GetFirmProductFields(int FirmProductIdx, int ProdcutFieldIdx);
        Task<bool> DeleteFirm(int Idx);
        Task<DataTableResponse<Tickets>> GetFirmTickets(IDataTableRequest spec, FirmTicketFilterModel filter);
    }
}
