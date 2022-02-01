using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{

    [Table("WorkersHistory", Schema = "task")]
    public class TaskWorkersHistories : BaseEntityModel
    {
        public long WorkerIdx { get; set; }
        public string WorkerData { get; set; }
        public string JobData { get; set; }
        public DateTime RunDate { get; set; }
        public string Params { get; set; }
        public string Result { get; set; }
        public int Status { get; set; }
    }
}
