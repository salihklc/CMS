using FluentValidation;

namespace CMS.Common.Models.ViewModels.Reports {
    public class DynamicReportModel {
        public int Idx { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportSql { get; set; }
        public string Parameters { get; set; }
    }

    public class DynamicReportValidator : AbstractValidator<DynamicReportModel> {
        public DynamicReportValidator () {
            RuleFor (x => x.ReportName).NotEmpty().Length(10, 200);
            RuleFor (x => x.Description).NotEmpty().Length(10, 200);
            RuleFor (x => x.ReportSql).NotEmpty().Length(5);
        }

    }
}