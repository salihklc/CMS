using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Products : BaseEntity, IAggregateRoot
    {
        public string Name_TR { get; set; }
        public string Name_EN { get; set; }
        public string Description_TR { get; set; }
        public string Description_EN { get; set; }
        public string ProductCode { get; set; }
        public ICollection<ProductFields> ProductFields { get; set; }
        public ICollection<FirmProducts> FirmProducts { get; set; }
    }
}
