using CMS.Core.Interfaces;


namespace CMS.Core.Entities {

    public class DynamicReports : BaseEntity, IAggregateRoot
    {
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportSql { get; set; }
        public string Parameters { get; set; }
    }
}