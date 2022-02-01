using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketAssigneeModel
    {
        public List<UserModel> Users { get; set; }
        public int AssigneeUserIdx { get; set; }
        public int UserIdx { get; set; }
        public int TicketIdx { get; set; }
        public string Description { get; set; }
    }

    public class TicketAssigneeModelValidator : AbstractValidator<TicketAssigneeModel>
    {
        public TicketAssigneeModelValidator()
        {
            RuleFor(x => x.AssigneeUserIdx).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
