using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.AppConstants;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketChangeStateModel
    {
        public int UserIdx { get; set; }
        public string Description { get; set; }
        public int StatusIdx { get; set; }
        public int TicketIdx { get; set; }
        public bool SendMailtoCustomer { get; set; }
        public int? TypeIdx { get; set; }
        public int CategoryIdx { get; set; }
    }

    public class TicketChangeStateModelValidator : AbstractValidator<TicketChangeStateModel>
    {
        public TicketChangeStateModelValidator()
        {
            RuleFor(x => x.StatusIdx).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.TypeIdx).NotEmpty().GreaterThan(0).When(r => r.CategoryIdx == (int)ITicketStatusCategory.DONE);
        }
    }
}
