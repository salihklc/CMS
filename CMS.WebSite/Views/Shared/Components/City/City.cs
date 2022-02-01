using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Core.Interfaces;

namespace CMS.WebSite.Views.Shared.Components.City
{
  
    public class City : ViewComponent
    {
        private readonly IGeneralServices _generalServices;
        public City(IGeneralServices generalServices)
        {
            _generalServices = generalServices;
        }
        public async Task<IViewComponentResult> InvokeAsync(string name,int selectedCityNo, int selectedDistrictNo)
        {
            CitiesViewModel citiesViewModel = new CitiesViewModel();
            citiesViewModel.Cities = await _generalServices.GetCities();
            citiesViewModel.PageModelName = name;
            citiesViewModel.SelectedCityNo = selectedCityNo;
            citiesViewModel.Districts = new DistrictViewModel();
            citiesViewModel.Districts.Districts = await _generalServices.GetDistricts(selectedCityNo);
            citiesViewModel.Districts.SelectedDistrictNo = selectedDistrictNo;
            citiesViewModel.Districts.PageModelName = "DistrictNo";

            return View(citiesViewModel);
        }
    }
}
