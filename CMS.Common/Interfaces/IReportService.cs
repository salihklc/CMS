using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Models.ViewModels.Reports;

namespace CMS.Common.Interfaces {
    public interface IReportService {
        Task<DataTableResponse<DynamicReportModel>> GetDynamicReports (IDataTableRequest spec);
        Task<List<DynamicReportModel>> GetDynamicReports ();
        Task<DynamicReportModel> GetDynamicReport (int Idx);
        Task<int> AddDynamicReport (DynamicReportModel model);
        Task<int> UpdateDynamicReport (DynamicReportModel model);
        Task<int> DeleteDynamicRepory (DynamicReportModel model);
        Task<object> RunReport (DynamicReportModel model);
    }
}