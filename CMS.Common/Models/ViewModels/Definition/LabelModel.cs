using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Definition
{
    public class LabelModel
    {
        public int Idx { get; set; }
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string Class { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public int InsertUserIdx { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class LabelModelValidator : AbstractValidator<LabelModel>
    {
        public LabelModelValidator()
        {
            RuleFor(x => x.Name_TR).NotEmpty();
            RuleFor(x => x.Name_EN).NotEmpty();          
        
        }
    }
}
