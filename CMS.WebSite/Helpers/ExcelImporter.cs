using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.WebSite.Helpers
{
    public class ExcelImporter
    {

        public static DataTable ReadDataFromStream(FileStream stream)
        {
            DataTable dataTable = new DataTable();
            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
            ISheet sheet = hssfwb.GetSheetAt(0);
            IRow headerRow = sheet.GetRow(0);
            // Headers
            foreach (var cell in headerRow.Cells)
            {
                dataTable.Columns.Add(cell.ToString(), typeof(string));
            }
            //Rows
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
               
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                List<string> rowValues = new List<string>();
                foreach (var cell in headerRow.Cells)
                {
                    rowValues.Add(row.GetCell(cell.ColumnIndex) == null ? "" : row.GetCell(cell.ColumnIndex).ToString());
                }
                dataTable.Rows.Add(rowValues.ToArray());
                
            }
            return dataTable;
        }
        public static string SaveFile(IFormFile formFile, string path, string nfileName)
        {

            string filePath = path;
            string fullPath = "";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (formFile.Length > 0)
            {           
                 fullPath = Path.Combine(filePath, nfileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                   
                }
            }
            return fullPath;
        }
    }
}
