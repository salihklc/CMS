using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Definition;
using CMS.Common.Models.ViewModels.Tickets;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class DefinitionService : IDefinitionService
    {
        private readonly IAsyncRepository<Labels> _labelRepository;
        private readonly IAsyncRepository<Priorities> _prioritiesRepository;
        private readonly IAsyncRepository<TicketTypes> _ticketTypeRepository;
        private readonly IAsyncRepository<TicketStatusCategories> _ticketStatusCategoriesRepository;
        private readonly IAsyncRepository<TicketStatus> _ticketStatusRepository;
        private readonly IAsyncRepository<WorkingTypes> _workingTypeRepository;
        public DefinitionService(IAsyncRepository<Labels> labelRepository, IAsyncRepository<Priorities> prioritiesRepository,
            IAsyncRepository<TicketTypes> ticketTypesRepository, IAsyncRepository<TicketStatusCategories> ticketStatusCategoriesRepository,
             IAsyncRepository<TicketStatus> ticketStatusRepository, IAsyncRepository<WorkingTypes> workingTypeRepository)
        {
            _labelRepository = labelRepository;
            _prioritiesRepository = prioritiesRepository;
            _ticketTypeRepository = ticketTypesRepository;
            _ticketStatusCategoriesRepository = ticketStatusCategoriesRepository;
            _ticketStatusRepository = ticketStatusRepository;
            _workingTypeRepository = workingTypeRepository;
        }

        public async Task<int> AddLabel(LabelModel model)
        {
            Labels label = model.xCopyTo<Labels>();
            await _labelRepository.AddAsync(label);
            return label.Idx;
        }

        public async Task<int> AddPriority(PriorityModel model)
        {
            Priorities priorities = model.xCopyTo<Priorities>();
            await _prioritiesRepository.AddAsync(priorities);
            return priorities.Idx;
        }

        public async Task<int> AddTicketStatus(TicketStatusModel model)
        {
            TicketStatus ticketStatus = model.xCopyTo<TicketStatus>();
            await _ticketStatusRepository.AddAsync(ticketStatus);
            return ticketStatus.Idx;
        }

        public async Task<int> AddTicketStatusCategory(TicketStatusCategoriesModel model)
        {
            TicketStatusCategories ticketStatusCategories = model.xCopyTo<TicketStatusCategories>();
            await _ticketStatusCategoriesRepository.AddAsync(ticketStatusCategories);
            return ticketStatusCategories.Idx;
        }

        public async Task<int> AddTicketType(TicketTypesModel model)
        {
            TicketTypes ticketTypes = model.xCopyTo<TicketTypes>();
            await _ticketTypeRepository.AddAsync(ticketTypes);
            return ticketTypes.Idx;
        }

        public async Task<int> AddWorkingType(WorkingTypesModel model)
        {
            WorkingTypes workingTypes = model.xCopyTo<WorkingTypes>();
            await _workingTypeRepository.AddAsync(workingTypes);
            return workingTypes.Idx;
        }

        public async Task<int> EditLabel(LabelModel model)
        {
            Labels labels = model.xCopyTo<Labels>();
            labels.UpdateDate = DateTime.Now;            
            await _labelRepository.UpdateAsync(labels);
            return labels.Idx;
        }

        public async Task<int> EditPriority(PriorityModel model)
        {
            Priorities priorities = model.xCopyTo<Priorities>();
            priorities.UpdateDate = DateTime.Now;          
            await _prioritiesRepository.UpdateAsync(priorities);
            return priorities.Idx;
        }

        public async Task<int> EditTicketStatus(TicketStatusModel model)
        {
            TicketStatus ticketStatus = model.xCopyTo<TicketStatus>();
            ticketStatus.UpdateDate = DateTime.Now;        
            await _ticketStatusRepository.UpdateAsync(ticketStatus);
            return ticketStatus.Idx;
        }

        public async Task<int> EditTicketStatusCategory(TicketStatusCategoriesModel model)
        {

            TicketStatusCategories ticketStatusCategories = model.xCopyTo<TicketStatusCategories>();
            ticketStatusCategories.UpdateDate = DateTime.Now;          
            await _ticketStatusCategoriesRepository.UpdateAsync(ticketStatusCategories);
            return ticketStatusCategories.Idx;
        }

        public async Task<int> EditTicketType(TicketTypesModel model)
        {
            TicketTypes ticketType = model.xCopyTo<TicketTypes>();
            ticketType.UpdateDate = DateTime.Now;
            ticketType.UpdateUserIdx = -1;
            await _ticketTypeRepository.UpdateAsync(ticketType);
            return ticketType.Idx; 
        }

        public async Task<int> EditWorkingType(WorkingTypesModel model)
        {
            WorkingTypes workingTypes = model.xCopyTo<WorkingTypes>();
            workingTypes.UpdateDate = DateTime.Now;
            workingTypes.UpdateUserIdx = model.UpdateUserIdx;
            await _workingTypeRepository.UpdateAsync(workingTypes);
            return workingTypes.Idx;
        }

        public async Task<LabelModel> GetLabel(int Idx)
        {
            var label = await _labelRepository.GetByIdAsync(Idx);
            var labelmodel = label.xCopyTo<LabelModel>();
            labelmodel.Status_Desc = labelmodel.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif";
            return labelmodel;
        }

        public async Task<DataTableResponse<LabelModel>> GetLabels(IDataTableRequest request)
        {
            var labels = await _labelRepository.ListAsync(request);

            DataTableResponse<LabelModel> returnModel = new DataTableResponse<LabelModel>();
            returnModel.Draw = labels.Draw;
            returnModel.RecordsFiltered = labels.RecordsFiltered;
            returnModel.RecordsTotal = labels.RecordsTotal;

            returnModel.Data = labels.Data.Select(f =>
            new LabelModel()
            {
                Idx = f.Idx,
                Name_TR = f.Name_TR,
                Name_EN = f.Name_EN,
                Description_EN = f.Description_EN,
                Description_TR = f.Description_TR,              
                Class = f.Class,
                Status_Desc= f.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif"
            }).ToList();

            return returnModel;
        }

        public async Task<DataTableResponse<PriorityModel>> GetPriorities(IDataTableRequest request)
        {
            var priorities = await _prioritiesRepository.ListAsync(request);

            DataTableResponse<PriorityModel> returnModel = new DataTableResponse<PriorityModel>();
            returnModel.Draw = priorities.Draw;
            returnModel.RecordsFiltered = priorities.RecordsFiltered;
            returnModel.RecordsTotal = priorities.RecordsTotal;

            returnModel.Data = priorities.Data.Select(f =>
            new PriorityModel()
            {
                Idx = f.Idx,
                Name_TR = f.Name_TR,
                Name_EN = f.Name_EN,
                Description_EN = f.Description_EN,
                Description_TR = f.Description_TR,
                Status_Desc = f.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif"
            }).ToList();

            return returnModel;
        }

        public async Task<PriorityModel> GetPriority(int Idx)
        {
            var priorities = await _prioritiesRepository.GetByIdAsync(Idx);
            var model = priorities.xCopyTo<PriorityModel>();
            model.Status_Desc = model.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif";
            return model;
        }

        public async Task<TicketStatusModel> GetTicketStatu(int Idx)
        {
            var ticketStatus = await _ticketStatusRepository.GetByIdAsync(Idx);
            var model = ticketStatus.xCopyTo<TicketStatusModel>();
            model.Status_Desc = model.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif";
            return model;
        }

        public async Task<DataTableResponse<TicketStatusModel>> GetTicketStatus(IDataTableRequest request)
        {
            var ticketstatus = await  _ticketStatusRepository.ListAsync(request);

            DataTableResponse<TicketStatusModel> returnModel = new DataTableResponse<TicketStatusModel>();
            returnModel.Draw = ticketstatus.Draw;
            returnModel.RecordsFiltered = ticketstatus.RecordsFiltered;
            returnModel.RecordsTotal = ticketstatus.RecordsTotal;

            returnModel.Data = ticketstatus.Data.Select(f =>
            new TicketStatusModel()
            {
                Idx = f.Idx,
                StatusName_TR = f.StatusName_TR,
                StatusName_EN = f.StatusName_EN,
                Description_EN = f.Description_EN,
                Description_TR = f.Description_TR,
                CategoryIdx = f.CategoryIdx,
                Status_Desc = f.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif"
        }).ToList();

            return returnModel;
        }

        public async Task<DataTableResponse<TicketStatusCategoriesModel>> GetTicketStatusCategories(IDataTableRequest request)
        {
            var ticketStatusCategories = await _ticketStatusCategoriesRepository.ListAsync(request);

            DataTableResponse<TicketStatusCategoriesModel> returnModel = new DataTableResponse<TicketStatusCategoriesModel>();
            returnModel.Draw = ticketStatusCategories.Draw;
            returnModel.RecordsFiltered = ticketStatusCategories.RecordsFiltered;
            returnModel.RecordsTotal = ticketStatusCategories.RecordsTotal;

            returnModel.Data = ticketStatusCategories.Data.Select(f =>
            new TicketStatusCategoriesModel()
            {
                Idx = f.Idx,
                StatusCategoryName_TR = f.StatusCategoryName_TR,
                StatusCategoryName_EN = f.StatusCategoryName_EN,
                Description_EN = f.Description_EN,
                Description_TR = f.Description_TR,
                Status =(int)GeneralStatus.Active,
                Status_Desc = (int)GeneralStatus.Active == 0 ? "Aktif" :"Pasif"
            }).ToList();

            return returnModel;
        }

        public async Task<TicketStatusCategoriesModel> GetTicketStatusCategory(int Idx)
        {
            var ticketStatusCategories = await _ticketStatusCategoriesRepository.GetByIdAsync(Idx);
            return ticketStatusCategories.xCopyTo<TicketStatusCategoriesModel>();
        }

        public async Task<DataTableResponse<TicketTypesModel>> GetTicketTypes(IDataTableRequest request)
        {
            var ticketTypes = await _ticketTypeRepository.ListAsync(request);

            DataTableResponse<TicketTypesModel> returnModel = new DataTableResponse<TicketTypesModel>();
            returnModel.Draw = ticketTypes.Draw;
            returnModel.RecordsFiltered = ticketTypes.RecordsFiltered;
            returnModel.RecordsTotal = ticketTypes.RecordsTotal;

            returnModel.Data = ticketTypes.Data.Select(f =>
            new TicketTypesModel()
            {
                Idx = f.Idx,
                TicketTypeName_TR = f.TicketTypeName_TR,
                TicketTypeName_EN = f.TicketTypeName_EN,
                Description_EN = f.Descrption_EN,
                Description_TR = f.Description_TR,
                Status_Desc = f.Status == (int)GeneralStatus.Active ? "Aktif" : "Pasif"
            }).ToList();

            return returnModel;
        }

        public async Task<TicketTypesModel> GetTicketTypes(int Idx)
        {
            var ticketTypes = await _ticketTypeRepository.GetByIdAsync(Idx);
            return ticketTypes.xCopyTo<TicketTypesModel>();
        }

        public async Task<WorkingTypesModel> GetWorkingType(int Idx)
        {
            var workingTypes = await _workingTypeRepository.GetByIdAsync(Idx);
            return workingTypes.xCopyTo<WorkingTypesModel>();
        }

        public async Task<DataTableResponse<WorkingTypesModel>> GetWorkingTypes(IDataTableRequest request)
        {
            var workingTypes = await _workingTypeRepository.ListAsync(request);

            DataTableResponse<WorkingTypesModel> returnModel = new DataTableResponse<WorkingTypesModel>();
            returnModel.Draw = workingTypes.Draw;
            returnModel.RecordsFiltered = workingTypes.RecordsFiltered;
            returnModel.RecordsTotal = workingTypes.RecordsTotal;

            returnModel.Data = workingTypes.Data.Select(f =>
            new WorkingTypesModel()
            {
                Idx = f.Idx,
                WorkingTypeName_TR = f.WorkingTypeName_TR,
                WorkingTypeName_EN = f.WorkingTypeName_EN,
                Status = f.Status,
                Status_Desc = f.Status == 0 ? "Aktif" : "Pasif"
            }).ToList();

            return returnModel;
        }

     
    }
}
