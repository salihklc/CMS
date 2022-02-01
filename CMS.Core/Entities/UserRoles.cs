using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class UserRoles : BaseEntity, IAggregateRoot
    {
        public int UserIdx { get; set; }
        public User User { get; set; }
        public int RoleIdx { get; set; }
        public Roles Roles { get; set; }
    }
}
