using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Fields: BaseEntity, IAggregateRoot
    {
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public int TypeIdx { get; set; }
        public FieldTypes Types { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
       public ICollection<ProductFields> ProductFields { get; set; }
    }
}
