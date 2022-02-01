using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Products
{
    public class FieldTypeModel
    {
        public int Idx { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserIdx { get; set; }
        public string InsertUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public string UpdateUserName { get; set; }
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
    }
}
