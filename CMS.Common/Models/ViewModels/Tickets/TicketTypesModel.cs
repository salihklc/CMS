using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketTypesModel
    {
        public int Idx { get; set; }
        public string TicketTypeName_TR { get; set; }
        public string TicketTypeName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string RecommendedActions { get; set; }
        public double EstimatedTime { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class TicketTypesModelValidator : AbstractValidator<TicketTypesModel>
    {
        public TicketTypesModelValidator()
        {
            RuleFor(x => x.TicketTypeName_TR).NotEmpty();
            RuleFor(x => x.TicketTypeName_EN).NotEmpty();         
            RuleFor(x => x.EstimatedTime).NotEmpty().GreaterThan(0);
         
        }
    }
}
