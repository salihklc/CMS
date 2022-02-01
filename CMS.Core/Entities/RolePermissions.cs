using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class RolePermissions :BaseEntity ,IAggregateRoot
    {
        public int RoleIdx { get; set; }
        public Roles Roles { get; set; }
        public int PermissionNo { get; set; }
        public Permissions Permissions { get; set; }
    }
}
