using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Pages : BaseEntity, IAggregateRoot
    {
        public string PageName { get; set; }
        public string Description { get; set; }
        public ICollection<Permissions> Permissions { get; set; }
    }
}
