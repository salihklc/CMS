using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Users
{
    public class UserModel
    {
        public int Idx { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string Address { get; set; }
        public int? CityNo { get; set; }
        public int? DistrictNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastSuccessLogin { get; set; }
        public int? DepartmanIdx { get; set; }
        public int? JobIdx { get; set; }
        public string DefaultLanguage { get; set; }
        public string Tags { get; set; }
        public string Picture { get; set; }
        public string PictureThumb { get; set; }
        public int? GroupIdx { get; set; }
        public int State { get; set; }
        public List<Permission.Permission> RolePermissions { get; set; }
        public string Permissions { get; set; }
    }
}
