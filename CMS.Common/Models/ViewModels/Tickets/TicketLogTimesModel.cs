using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.Models.ViewModels.Users;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class TicketLogTimesModel
    {
        public int Idx { get; set; }
        public int TicketIdx { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double LogTime { get; set; }
        public string Comment { get; set; }
        public int WorkingTypeIdx { get; set; }
        public WorkingTypesModel WorkingTypesModel { get; set; }
        public int UserIdx { get; set; }
        public UserModel UserModel { get; set; }
    }

    public class TicketLogTimesModelValidator : AbstractValidator<TicketLogTimesModel>
    {
        public TicketLogTimesModelValidator()
        {
            RuleFor(x => x.LogTime).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.TicketIdx).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}

