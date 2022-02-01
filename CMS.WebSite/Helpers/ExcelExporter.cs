using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Ahtapot.WebSite.Helpers
{

    public class ExcelExporter
    {

        public static MemoryStream GetExcelExportMemory<T>(List<T> dataList) where T : class
        {
            IWorkbook workbook;
     
               
                workbook = new XSSFWorkbook();

                ISheet sheet = workbook.CreateSheet("Sayfa 1");

                //var type_ = typeof(T);
                //var members = type_.GetCustomAttributes(true);

                //foreach (var item in members)
                //{
                //    var a = item as DisplayNameAttribute;
                //    if (a != null)
                //    {
                //        a.DisplayName
                //    }
                //}

                var _headers = new List<string>();
                var _type = new List<string>();

                //foreach (var item in members)
                //{
                //    // item.GetCustomAttributes();
                //}

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

                DataTable table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                {
                    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    _type.Add(type.Name);
                    var dname= prop.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                   
                    table.Columns.Add(prop.DisplayName, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                      prop.PropertyType);
                    string name = Regex.Replace(dname != null ? dname.Name : prop.DisplayName, "([A-Z])", " $1").Trim();
                    //name by caps for header
                    _headers.Add(name);
                }

                foreach (T item in dataList)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }



                for (int i = 0; i < table.Rows.Count; i++)
                {
                    IRow sheetRow = sheet.CreateRow(i + 1);
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        ICell Row1 = sheetRow.CreateCell(j);
                        string cellvalue = Convert.ToString(table.Rows[i][j]);

                        // TODO: move it to switch case

                        if (string.IsNullOrWhiteSpace(cellvalue))
                        {
                            Row1.SetCellValue(string.Empty);
                        }
                        else if (_type[j].ToLower(cultureInfo) == "string")
                        {
                            Row1.SetCellValue(cellvalue);
                        }
                        else if (_type[j].ToLower(cultureInfo) == "int32")
                        {
                            Row1.SetCellValue(Convert.ToInt32(table.Rows[i][j]));
                        }
                        else if (_type[j].ToLower(cultureInfo) == "double")
                        {
                            Row1.SetCellValue(Convert.ToDouble(table.Rows[i][j]));
                        }
                        else if (_type[j].ToLower(cultureInfo) == "datetime")
                        {
                            Row1.SetCellValue(Convert.ToDateTime
                                 (table.Rows[i][j]).ToString("dd/MM/yyyy hh:mm:ss"));
                        }
                        else
                        {
                            Row1.SetCellValue(string.Empty);
                        }
                    }
                }
                //foreach (var rowData in dataList)
                //{
                //    // rowData.;
                //}
                //Header
                var header = sheet.CreateRow(0);
                for (var i = 0; i < _headers.Count; i++)
                {
                    var cell = header.CreateCell(i);
                    cell.SetCellValue(_headers[i]);

                }

            
            //var memoryStream = new MemoryStream();
            //using (var stream = new FileStream(Path.Combine(webRootPath, sFileName), FileMode.Open))
            //{
            //    stream.CopyTo(memoryStream);
            //}

            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;

            //MemoryStream m = new MemoryStream();
         
        }

    }
}