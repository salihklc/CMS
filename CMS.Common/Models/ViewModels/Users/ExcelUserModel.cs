using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Users
{
    public class ExcelUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string Address { get; set; }
        public int? CityNo { get; set; }
        public int? DistrictNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastSuccessLogin { get; set; }
        public string DefaultLanguage { get; set; }
        public int State { get; set; }
        public string Firm { get; set; }
        public int? FirmIdx { get; set; }
    }
}
