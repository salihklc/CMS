using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.Common.Models.ViewModels.Products
{
    public class AddProductModel
    {
        public int Idx { get; set; }
        public int UserIdx { get; set; }
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserIdx { get; set; }
        public List<int> Fields { get; set; }
        public ProductFieldsModel SelectedFields { get; set; }
    }

    public class AddProductModelValidator : AbstractValidator<AddProductModel>
    {
        private readonly IProductService _productService;
        public AddProductModelValidator(IProductService productService)
        {
            _productService = productService;
            RuleFor(x => x.Name_TR).NotEmpty();
            RuleFor(x => x.Name_EN).NotEmpty().MustAsync((x, FirmName, cancellation) => IsNameUnique(x.Name_TR, x.Name_EN,x.Idx));
           
        }

        public async Task<bool> IsNameUnique(string Name_TR, string Name_EN,int Idx)
        {
            return !(await _productService.IsNameUnique(Name_TR, Name_EN,Idx));
        }
     
    }
}
