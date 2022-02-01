using System;

namespace CMS.Core.Entities
{
    public class BaseEntity
    {
        public int Idx { get; set; }
        public int InsertUserIdx { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
    }

}