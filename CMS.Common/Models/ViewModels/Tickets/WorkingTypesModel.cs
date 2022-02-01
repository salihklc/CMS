using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class WorkingTypesModel
    {
        public int Idx { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string WorkingTypeName_TR { get; set; }
        public string WorkingTypeName_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string Color { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
    }
}
