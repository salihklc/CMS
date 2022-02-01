using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.TaskManager.Business.Logic.Models
{
    public enum DayOfWeeks
    {
        Monday = 1,
        Tuesday,
        Wednesday,
        Thurday,
        Friday,
        Saturday,
        Sunday
    }
    public class CommonModels
    {

    }

    public class RestCallResponse
    {
        public string ResultData { get; set; }
        public string HttpStatus { get; set; }
        public string RequestData { get; set; }
    }

    public class ApplicationConfig
    {
        public int QueueReaderTimer { get; set; }
        public int WatchTimer { get; set; }
    }

    public class ConnectionStrings
    {
        public string CmsDatabase { get; set; }
    }
}
