using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Roles;

namespace CMS.Common.Models.ViewModels.Users
{

    public class CreateUserModel
    {
        public CreateUserModel()
        {
            SelectedRoles = new string[] { };
        }

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
        public int? DepartmanIdx { get; set; }
        public int? JobIdx { get; set; }
        public string DefaultLanguage { get; set; }
        public string Tags { get; set; }
        public string Picture { get; set; }
        public IFormFile PictureData { get; set; }
        public string PictureThumb { get; set; }
        public int? GroupIdx { get; set; }
        public int State { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public string[] SelectedRoles { get; set; }
        public string[] SelectedGroups { get; set; }
        public string TaxNo { get; set; }
        public string TcNo { get; set; }
        public int? FirmIdx { get; set; }
    }

    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        private readonly IUserService _userService;
        public CreateUserModelValidator(IUserService userService)
        {
            _userService = userService;
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Gsm).NotEmpty().MinimumLength(10).MaximumLength(20);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MustAsync((x, cancellation) => EmailIsUnique(x));
            RuleFor(x => x.Password).NotEmpty().Must(ValidPassword);

        }

        private bool ValidPassword(string Password)
        {
            return true;
        }
        private async Task<bool> EmailIsUnique(string email)
        {
            return (await _userService.EmailIsUnique(email));
        }
    }

}