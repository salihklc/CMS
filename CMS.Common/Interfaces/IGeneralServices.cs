using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Common.Models.ViewModels.Products;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Common.Models.ViewModels.Tickets;

namespace CMS.Common.Interfaces
{
    public interface IGeneralServices
    {
        Task<List<City>> GetCities();
        Task<List<District>> GetDistricts(int cityNo);
        Task<List<SelectModel>> GetCitiesSelect();
        Task<List<SelectModel>> GetDistrictsSelect(int cityNo);
        Task<List<SelectModel>> GetRoles();
        Task<List<SelectModel>> GetPriorities();
        Task<List<SelectModel>> GetTicketTypes();
        Task<List<SelectModel>> GetWorkingTypes();
        Task<List<SelectModel>> GetTicketUsers();
        Task<List<SelectModel>> GetTicketUsers(int TicketIdx);
        Task<List<SelectModel>> GetTicketStatus();
        Task<List<SelectModel>> GetTicketStatusCategories();
        Task<List<SelectModel>> GetFirms();
        Task<List<SelectModel>> GetEnvironment();
        Task<List<SelectModel>> GetRelatedTickets(int FirmIdx);
        Task<List<SelectModel>> GetFieldTypes();
        Task<List<SelectModel>> GetFields();
        Task<List<FieldsModel>> GetFieldsAllData(int productIdx =0);
        Task<List<FieldsModel>> GetSelectedFieldsAllData(int productIdx);
        Task<List<SelectModel>> GetProducts();
        Task<List<FieldsModel>> GetProductFields(int productIdx);
        Task<List<Models.ViewModels.Firms.ProductFieldsModel>> GetFirmProductFields(int firmProductIdx);
    }

}
