using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{
    public class BaseEntityModel
    {
        [Key]
        public long Idx { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public int UpdateUserIdx { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
