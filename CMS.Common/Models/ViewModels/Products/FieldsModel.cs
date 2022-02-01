using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Products
{
    public class FieldsModel
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
        public int TypeIdx { get; set; }
        public string TypeName { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
    }
}
