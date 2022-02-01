using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using CMS.Common.Models.ViewModels.Roles;

namespace CMS.Common.Models.ViewModels.Users
{
    public class UserViewModel
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
        public IFormFile PictureData { get; set; }
        public string PictureThumb { get; set; }
        public int? GroupIdx { get; set; }
        public int State { get; set; }
        public List<UserRolesViewModel> UserRoles { get; set; }
        public string[] SelectedRoles { get; set; }
        public string[] SelectedGroups { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public int? FirmIdx { get; set; }
        public UserViewModel()
        {
            UserRoles = new List<UserRolesViewModel>();
        }
    }
}