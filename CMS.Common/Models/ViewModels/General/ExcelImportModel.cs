using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.General
{
    public class ExcelImportModel
    {
        public string ExampleExcelPath { get; set; }
        public string UploadExcelPath { get; set; }
        public string PostExcelFileUrl { get; set; }
        public string ExtraData { get; set; }
        
    }
}
