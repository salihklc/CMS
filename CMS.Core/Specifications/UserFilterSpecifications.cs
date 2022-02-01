using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Core.Specifications
{
    public class UserFilterSpecifications :BaseSpecification<User>
    {
        public UserFilterSpecifications(int userIdx)
                   : base(o => o.Idx == userIdx)
        {
            AddInclude(o => o.UserRoles);
        
        }
    }
}
