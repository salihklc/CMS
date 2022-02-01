using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Firms : BaseEntity , IAggregateRoot
    {
        public int? FirmNo { get; set; }
        public string FirmName { get; set; }
        public string CommercialTitle { get; set; }
        public string TaxNo { get; set; }
        public string TcNo { get; set; }
        public string Address { get; set; }
        public int? CityNo { get; set; }
        public int? DistrictNo { get; set; }
        public string Gsm { get; set; }
        public string Gsm2 { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string Website { get; set; }
        //Default firm Mail ticket için
        public bool? IsDefault { get; set; }
        public ICollection<Tickets> Tickets { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<FirmProducts> FirmProducts { get; set; }
        
    }
}
