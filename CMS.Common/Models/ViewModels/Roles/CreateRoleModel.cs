using FluentValidation;
using System;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.Common.Models.ViewModels.Roles
{
    public class CreateRoleModel
    {
        public int Idx { get; set; }
        public string RoleName { get; set; } 
        public string Description { get; set; }
        public int State { get; set; }
    }

    public class CreateRoleModelValidator : AbstractValidator<CreateRoleModel>{
        private readonly IRoleService _roleService;
        public CreateRoleModelValidator(IRoleService roleService)
        {
            _roleService = roleService;
            RuleFor(x => x.RoleName).NotEmpty().MustAsync((x, cancellation) => IsUniqueRoleName(x));
            RuleFor(x => x.Description).NotEmpty();
        }
        public async Task<bool> IsUniqueRoleName(string roleName)
        {
            return !(await _roleService.IsUniqueRoleName(roleName));
        }
    }

}