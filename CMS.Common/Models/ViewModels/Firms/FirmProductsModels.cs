using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Products;

namespace CMS.Common.Models.ViewModels.Firms
{
    public class FirmProductsModels
    {
        public int Idx { get; set; }
        public int FirmIdx { get; set; }
        public string FirmName { get; set; }
        public string TaxNo { get; set; }
        public int ProductIdx { get; set; }
        public string ProductName_TR { get; set; }
        public string ProductName_EN { get; set; }
    }
}
