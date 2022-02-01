using System.Collections.Generic;
using Newtonsoft.Json;

namespace CMS.Common.Interfaces
{


    public class DTParameterModel : IDataTableRequest
    {
        public bool GetAll { get; set; }
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DTSearch Search { get; set; }

        public IEnumerable<DTOrder> Order { get; set; }

        public IEnumerable<DTColumn> Columns { get; set; }
    }

    public sealed class DTSearch
    {
        public string Value { get; set; }

        public bool Regex { get; set; }
    }

    public sealed class DTOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public sealed class DTColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public DTSearch Search { get; set; }
    }


    public interface IDataTableRequest
    {
        bool GetAll { get; set; }
        int Draw { get; set; }
        int Start { get; set; }
        int Length { get; set; }
        DTSearch Search { get; set; }

        IEnumerable<DTOrder> Order { get; set; }

        IEnumerable<DTColumn> Columns { get; set; }
    }


    public class DataTableResponse<T>
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("data")]
        public IList<T> Data { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }
    }
}