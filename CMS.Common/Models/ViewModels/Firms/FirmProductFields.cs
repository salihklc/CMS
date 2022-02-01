using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Firms
{
    public class FirmProductFieldsModel
    {
        public int FirmIdx { get; set; }
        public string FirmName { get; set; }
        public int ProductIdx { get; set; }
        public string ProductName { get; set; }
        public int UserIdx { get; set; }
        public int FirmProductIdx { get; set; }
        public List<ProductFieldsModel> ProductFields { get; set; }
    }

    public class ProductFieldsModel
    {
        public int FieldIdx { get; set; }
        public string Value { get; set; }
    }
}
