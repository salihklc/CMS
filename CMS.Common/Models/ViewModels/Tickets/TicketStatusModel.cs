using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketStatusModel
    {
        public int Idx { get; set; }
        public string StatusName_TR { get; set; }
        public string StatusName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public int CategoryIdx { get; set; }     
        public int InsertUserIdx { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime? UpdateDate { get; set; }

    }

    public class TicketStatusValidator : AbstractValidator<TicketStatusModel>
    {
        public TicketStatusValidator()
        {
            RuleFor(x => x.StatusName_TR).NotEmpty();
            RuleFor(x => x.StatusName_EN).NotEmpty();
            RuleFor(x => x.CategoryIdx).GreaterThan(0);
        }
    }
}
