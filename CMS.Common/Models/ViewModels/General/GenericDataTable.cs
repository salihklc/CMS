using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.General
{
    public class GenericDataTable
    {
        public string Colums { get; set; }
        public string Rows { get; set; }
        public ExcelImportModel ExcelImportModel { get; set; }
    }
    
}
