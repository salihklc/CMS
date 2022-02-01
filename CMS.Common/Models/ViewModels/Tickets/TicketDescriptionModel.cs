using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketDescriptionModel
    {
        public int TicketIdx { get; set; }
        public string Description { get; set; }
        public int UserIdx { get; set; }

    }
    public class TicketDescriptionModelValidator : AbstractValidator<TicketDescriptionModel>
    {
        public TicketDescriptionModelValidator()
        {
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
