using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using CMS.Common.Interfaces;
using CMS.Core.Entities;

namespace CMS.Storage.Repositories
{
    public class DataTableEvaluator<T> where T : BaseEntity
    {
        public static DataTableResponse<T> GetQueryable(IQueryable<T> baseQuery, IDataTableRequest dataTableRequest)
        {
            var resultModel = new DataTableResponse<T>();
            resultModel.RecordsTotal = baseQuery.Count();

            // try
            // {
            if (!dataTableRequest.GetAll)
            {

                #region Top search query (searching for all columns)
                string whereOrQuery = "";
                string value = "";
                bool hasBool = false;
                bool hasDate = false;

                if (dataTableRequest.Search != null && !string.IsNullOrEmpty(dataTableRequest.Search.Value))
                {
                    if (dataTableRequest.Columns.Count() > 0)
                    {
                        int columnCounter = 0;
                        foreach (var item in dataTableRequest.Columns)
                        {
                            var propName = typeof(T).GetProperty(item.Data);

                            var typeCode = Type.GetTypeCode(propName.PropertyType);


                            value = dataTableRequest.Search.Value;


                            switch (typeCode)
                            {
                                case TypeCode.String:
                                    whereOrQuery += String.Format("{0}.Contains(@0)", item.Data);
                                    break;
                                case TypeCode.Boolean:

                                    whereOrQuery += String.Format("{0} == @1", item.Data);

                                    hasBool = true;

                                    break;
                                case TypeCode.Int16:
                                case TypeCode.Int32:
                                    whereOrQuery += String.Format("{0}.ToString().Contains(@0)", item.Data);
                                    break;
                                case TypeCode.Int64:
                                case TypeCode.UInt16:
                                case TypeCode.UInt32:
                                case TypeCode.UInt64:
                                    whereOrQuery += String.Format("{0}.ToString().Contains(@0)", item.Data);
                                    break;
                                case TypeCode.DateTime:
                                    whereOrQuery += String.Format("{0}.ToString('yyyyMMddhhmmss').Contains(@2)", item.Data);

                                    hasDate = true;
                                    break;
                            }

                            columnCounter++;

                            if (columnCounter != dataTableRequest.Columns.Count())
                                whereOrQuery += " || ";
                        }

                        var boolValue = (value != null && (value == "1" || value.ToLower() == "true" || value.ToLower() == "evet" || value.ToLower() == "yes"))
                                                                  ? true
                                                                  : false;
                        if (!hasBool && !hasDate)
                            baseQuery = baseQuery.Where(whereOrQuery, value);
                        if (hasBool && !hasDate)
                            baseQuery = baseQuery.Where(whereOrQuery, value, "", Regex.Replace(value, "\\D+", ""));
                        if (!hasBool && hasDate)
                            baseQuery = baseQuery.Where(whereOrQuery, value, boolValue);
                        if (hasBool && hasDate)
                            baseQuery = baseQuery.Where(whereOrQuery, value, boolValue, Regex.Replace(value, "\\D+", ""));


                    }
                }
                #endregion



                #region individual column search
                if (dataTableRequest.Columns !=null && dataTableRequest.Columns.Count() > 0)
                {
                    foreach (var item in dataTableRequest.Columns)
                    {
                        var propName = typeof(T).GetProperty(item.Data);

                        if (propName != null)
                        {
                            var typeCode = Type.GetTypeCode(propName.PropertyType);

                            value = "";

                            if (item.Searchable && !string.IsNullOrEmpty(item.Search.Value))
                            {
                                value = item.Search.Value;

                                if (!string.IsNullOrEmpty(value))
                                {

                                    switch (typeCode)
                                    {
                                        case TypeCode.String:
                                            baseQuery = baseQuery.Where(String.Format("{0}.Contains(@0)", item.Data), value);
                                            break;
                                        case TypeCode.Boolean:
                                            var boolValue = (value != null
                                                && (value == "1" || value.ToLower() == "true"))
                                                ? true
                                                : false;
                                            baseQuery = baseQuery.Where(String.Format("{0} == @0", item.Data), boolValue);
                                            break;
                                        case TypeCode.Int16:
                                        case TypeCode.Int32:
                                            baseQuery = baseQuery.Where(String.Format("{0}.ToString().Contains(@0)", item.Data), value);
                                            break;
                                        case TypeCode.Int64:
                                        case TypeCode.UInt16:
                                        case TypeCode.UInt32:
                                        case TypeCode.UInt64:
                                            baseQuery = baseQuery.Where(String.Format("{0}.ToString().Contains(@0)", item.Data), value);
                                            break;
                                        case TypeCode.DateTime:
                                            baseQuery = baseQuery.Where(String.Format("{0}.ToString('yyyyMMddhhmmss').Contains(@0)", item.Data), Regex.Replace(value, "\\D+", ""));
                                            break;
                                    }
                                }

                            }
                        }

                    }
                }
                #endregion


                if (dataTableRequest.Order !=null && dataTableRequest.Order.Count() > 0)
                {
                    int orderCounter = 0;
                    string orderStr = "";
                    var columnAsList = dataTableRequest.Columns.ToList();

                    foreach (var item in dataTableRequest.Order)
                    {
                        var columnName = columnAsList[item.Column].Data;
                        orderStr += columnName;
                        orderStr += " " + item.Dir;

                        orderCounter++;

                        if (dataTableRequest.Order.Count() != orderCounter)
                        {
                            orderStr += ",";
                        }
                    }

                    baseQuery = baseQuery.OrderBy(orderStr);
                }

                resultModel.RecordsFiltered = baseQuery.Count();

                baseQuery = baseQuery.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);
            }
            // }
            // catch (System.Exception ex)
            // {
            //     Log
            // }

            resultModel.Data = baseQuery.ToList();

            return resultModel;
        }
    }
}