using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CMS.Common.Interfaces;

namespace CMS.Common.Models.ViewModels.Firms
{
  
    public class FirmsModel
    {
        public int Idx { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserIdx { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserIdx { get; set; }
        public int? FirmNo { get; set; }
        public string FirmName { get; set; }
        public string CommercialTitle { get; set; }
        public string TaxNo { get; set; }
        public string TcNo { get; set; }
        public string Address { get; set; }
        public int? CityNo { get; set; }
        public int? DistrictNo { get; set; }
        public string Gsm { get; set; }
        public string Gsm2 { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string Website { get; set; }
        public int Status { get; set; }
        public string Status_Desc { get; set; }
        public bool? IsDefault { get; set; }
    
    }
    public class FirmsModelValidator : AbstractValidator<FirmsModel>
    {
        private readonly IFirmService _firmService;
        public FirmsModelValidator(IFirmService firmService)
        {
            _firmService = firmService;
            RuleFor(x => x.Gsm).NotEmpty().Length(10,16);
            RuleFor(x => x.FirmName).NotEmpty().MustAsync((x,FirmName, cancellation) => IsFirmNameUnique(x.FirmName,x.Idx));
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.TaxNo).NotEmpty().Length(10, 10).MustAsync((x,TaxNo, cancellation) => IsTaxNoUnique(x.TaxNo,x.Idx));
        }

        public async Task<bool> IsTaxNoUnique(string taxNo,int Idx)
        {
            return !(await _firmService.TaxNoIsUnique(taxNo,Idx));
        }
        public async Task<bool> IsFirmNameUnique(string firmName,int Idx)
        {
            return !(await _firmService.FirmNameIsUnique(firmName,Idx));
        }
    }
}
