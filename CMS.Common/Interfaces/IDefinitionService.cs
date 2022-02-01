using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Definition;
using CMS.Common.Models.ViewModels.Tickets;

namespace CMS.Common.Interfaces
{
    public interface IDefinitionService
    {
        Task<DataTableResponse<LabelModel>> GetLabels(IDataTableRequest request);
        Task<LabelModel> GetLabel(int Idx);
        Task<int> EditLabel(LabelModel model);
        Task<int> AddLabel(LabelModel model);

        Task<DataTableResponse<PriorityModel>> GetPriorities(IDataTableRequest request);
        Task<PriorityModel> GetPriority(int Idx);
        Task<int> EditPriority(PriorityModel model);
        Task<int> AddPriority(PriorityModel model);

        Task<DataTableResponse<TicketTypesModel>> GetTicketTypes(IDataTableRequest request);
        Task<TicketTypesModel> GetTicketTypes(int Idx);
        Task<int> EditTicketType(TicketTypesModel model);
        Task<int> AddTicketType(TicketTypesModel model);

        Task<DataTableResponse<TicketStatusCategoriesModel>> GetTicketStatusCategories(IDataTableRequest request);
        Task<TicketStatusCategoriesModel> GetTicketStatusCategory(int Idx);
        Task<int> EditTicketStatusCategory(TicketStatusCategoriesModel model);
        Task<int> AddTicketStatusCategory(TicketStatusCategoriesModel model);

        Task<DataTableResponse<TicketStatusModel>> GetTicketStatus(IDataTableRequest request);
        Task<TicketStatusModel> GetTicketStatu(int Idx);
        Task<int> EditTicketStatus(TicketStatusModel model);
        Task<int> AddTicketStatus(TicketStatusModel model);

        Task<DataTableResponse<WorkingTypesModel>> GetWorkingTypes(IDataTableRequest request);
        Task<WorkingTypesModel> GetWorkingType(int Idx);
        Task<int> EditWorkingType(WorkingTypesModel model);
        Task<int> AddWorkingType(WorkingTypesModel model);
    }
}
