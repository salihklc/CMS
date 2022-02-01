using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.TaskManager.Business.Database.EntityModels
{
    [Table("Jobs",Schema ="task")]
    public class TaskJobs : BaseEntityModel
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string RequiredParams { get; set; }
    }
}
