using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Products
{
    public class ProductFieldsModel
    {
        public int ProductIdx { get; set; }     
        public List<FieldsModel> FieldsModels { get; set; }
    }
}
