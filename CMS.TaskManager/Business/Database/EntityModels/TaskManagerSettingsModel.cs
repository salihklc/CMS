using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{

    [Table("TaskManagerSettings")]
    public class TaskManagerSettingsModel:BaseEntityModel
    {
        public int WatcherTimer { get; set; }
        public int QueueTimer { get; set; }
    }
}
