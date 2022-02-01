using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketStatusCategoriesModel
    {
        public int Idx { get; set; }
        public string StatusCategoryName_TR { get; set; }
        public string StatusCategoryName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public int InsertUserIdx { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
    }

    public class TicketStatusCategoriesValidator : AbstractValidator<TicketStatusCategoriesModel>
    {
        public TicketStatusCategoriesValidator()
        {
            RuleFor(x => x.StatusCategoryName_TR).NotEmpty();
            RuleFor(x => x.StatusCategoryName_EN).NotEmpty();
        }
    }
}
