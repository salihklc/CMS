using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Tickets
{
    public class CreateTicketModel
    {
        public int InsertUserIdx { get; set; }
        public DateTime InsertDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public AddTicketAttachments[] TicketAttachments { get; set; }
        public int? TypeIdx { get; set; }
        public int FirmIdx { get; set; }
        public int? FirmUserIdx { get; set; }
        public int? FirmProjectIdx { get; set; }
        public int? EnvironmentIdx { get; set; }
        public string DocumentLinks { get; set; }
        public int? PriorityIdx { get; set; }       
        public List<IFormFile> Attachments { get; set; }

    }
    public class AddTicketAttachments
    {
        public string AttachmentName { get; set; }
        public string AttachmentUrl { get; set; }
        public string FileType { get; set; }
    }


    public class CreateTicketModelValidator : AbstractValidator<CreateTicketModel>
    {     
        public CreateTicketModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PriorityIdx).NotEmpty().GreaterThan(0);        
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.FirmIdx).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FirmUserIdx).NotEmpty().GreaterThan(0);
        }      
    }
}
