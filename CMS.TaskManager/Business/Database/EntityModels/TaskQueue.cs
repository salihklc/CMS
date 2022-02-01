using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{

    [Table("Queue", Schema = "task")]
    public class TaskQueue : BaseEntityModel
    {
        public long WorkerIdx { get; set; }
        public int Status { get; set; } // 0 waiting, 1 inprocess
    }
}
