using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class FirmProducts : BaseEntity, IAggregateRoot
    {
        public int FirmIdx { get; set; }
        public Firms Firms { get; set; }
        public int ProductIdx { get; set; }
        public Products Products { get; set; }
        public ICollection<FirmProductFields> FirmProductFields { get; set; }
    }
}
