using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Firms;
using CMS.Common.Models.ViewModels.Tickets;

namespace CMS.Common.Interfaces
{
    public interface IFirmService
    {
        Task<DataTableResponse<FirmsModel>> GetFirms(IDataTableRequest request);
        Task<List<FirmsModel>> GetFirms();
        Task<FirmsModel> GetFirm(int Idx);
        Task<int> AddFirm(FirmsModel model);
        Task<bool> TaxNoIsUnique(string taxNo,int Idx);
        Task<bool> FirmNameIsUnique(string firmName, int Idx);
        Task<int> EditFirm(FirmsModel model);
        Task<bool> DeleteFirm(int Idx);
        Task<int> AddFirms(List<FirmsModel> model);
        Task<DataTableResponse<FirmProductsModels>> GetFirmProducts(IDataTableRequest request,int FirmIdx);
        Task<List<FirmProductsModels>> GetFirmProducts(int FirmIdx);
        Task<int> AddFirmProducts(FirmProductFieldsModel model);
        Task<FirmProductsModels> GetFirmProductsModels(int firmProductIdx);
        Task<int> EditFirmProducts(FirmProductFieldsModel model);
        Task<DataTableResponse<TicketsModel>> GetFirmTickets(IDataTableRequest request, FirmTicketFilterModel filter);
        Task<List<TicketsModel>> GetFirmTickets(int FirmIdx);
    }
}
