using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class UserHistories : BaseEntity, IAggregateRoot
    {
        public int UserIdx { get; set; }
        public string CdcData { get; set; }
    }
}
