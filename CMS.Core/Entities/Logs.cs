using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Logs : BaseEntity,IAggregateRoot
    {
        public string Message { get; set; }
        public string Template { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string Application { get; set; }

    }
}
