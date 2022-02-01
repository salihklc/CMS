using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Core.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.District
{
    public class District : ViewComponent
    {
        private readonly IGeneralServices _generalServices;
        public District(IGeneralServices generalServices)
        {
            _generalServices = generalServices;
        }
        public async Task<IViewComponentResult> InvokeAsync(string name, int selecteddistrictNo,int cityNo)
        {
            DistrictViewModel districtViewModel = new DistrictViewModel();
            districtViewModel.Districts = await _generalServices.GetDistricts(cityNo);
            districtViewModel.PageModelName = name;
            districtViewModel.SelectedDistrictNo = selecteddistrictNo;
            return View(districtViewModel);
        }
    }
}
