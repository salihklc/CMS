using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{

    [Table("Workers", Schema = "task")]
    public class TaskWorkers : BaseEntityModel
    {
        public long JobIdx { get; set; }
        public DateTime LastRunDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ScheduleTime { get; set; }
        public int DayOfWeek { get; set; }
        public string Params { get; set; }
        public TimeSpan StartTimeOfDay { get; set; }
        public TimeSpan EndTimeOfDay { get; set; }

    }
}
