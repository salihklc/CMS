using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class ProductFields : BaseEntity , IAggregateRoot
    {        
        public int ProductIdx { get; set; }
        public Products Products { get; set; }
        public int FieldIdx { get; set; }
        public Fields Fields { get; set; }
        public ICollection<FirmProductFields> FirmProductFields { get; set; }
    }
}
