using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class UserGroups : BaseEntity, IAggregateRoot
    {
        public string GroupName { get; set; }
    }
}
