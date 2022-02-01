using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.AppConstants;
using CMS.Common.Extensions;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Common.Models.ViewModels.Products;
using CMS.Common.Models.ViewModels.Reports;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Core.Entities;
using CMS.Core.Interfaces;

namespace CMS.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly IAsyncRepository<DynamicReports> _dynamicReportRepo;
        public ReportService(
            ILogger<HomeService> logger,
            IAsyncRepository<DynamicReports> dynamicReportRepo
        )
        {
            _logger = logger;
            _dynamicReportRepo = dynamicReportRepo;
        }

         public async Task<DataTableResponse<FieldsModel>> GetFields(IDataTableRequest request)
        {
            var fields = await _dynamicReportRepo.ListAsync(request);

            DataTableResponse<FieldsModel> returnModel = new DataTableResponse<FieldsModel>();
            returnModel.Draw = fields.Draw;
            returnModel.RecordsFiltered = fields.RecordsFiltered;
            returnModel.RecordsTotal = fields.RecordsTotal;

            returnModel.Data = fields.Data.Select(f =>
            new FieldsModel()
            {
                Idx = f.Idx,
                InsertDate = f.InsertDate,
                InsertUserIdx = f.InsertUserIdx
            }).ToList();
            return returnModel;
        }

        public async Task<Common.Models.ViewModels.Reports.DynamicReportModel> GetDynamicReports()
        {
            Common.Models.ViewModels.Reports.DynamicReportModel homeCounters = new Common.Models.ViewModels.Reports.DynamicReportModel();

            try
            {
                DTParameterModel requestModel = new DTParameterModel() { Length = 1 };

                var myReports = await _dynamicReportRepo.ListAsync(requestModel);

                // var result = await Task.WhenAll(
                //     myFirmRepoEntity,
                //     myTicketEntity,
                //     myUserEntity,
                //     myProductEntity);


                return homeCounters;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "HATA {msg}", ex.Message);
            }

            return homeCounters;
        }

        public Task<DataTableResponse<DynamicReportModel>> GetDynamicReports(IDataTableRequest spec)
        {
            throw new System.NotImplementedException();
        }

        Task<List<DynamicReportModel>> IReportService.GetDynamicReports()
        {
            throw new System.NotImplementedException();
        }

        public Task<DynamicReportModel> GetDynamicReport(int Idx)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddDynamicReport(DynamicReportModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateDynamicReport(DynamicReportModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteDynamicRepory(DynamicReportModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> RunReport(DynamicReportModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
