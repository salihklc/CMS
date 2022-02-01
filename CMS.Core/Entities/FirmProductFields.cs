using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class FirmProductFields : BaseEntity , IAggregateRoot
    {
        public int FirmProductIdx { get; set; }
        public FirmProducts FirmProducts { get; set; }
        public int ProductFieldIdx { get; set; }
        public ProductFields ProductFields { get; set; }
        public string Value { get; set; }
     
    }
}
