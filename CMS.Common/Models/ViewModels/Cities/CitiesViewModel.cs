using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.ViewModels.Cities
{
    public class CitiesViewModel
    {
        public List<City> Cities { get; set; }
        public int SelectedCityNo { get; set; }
        public string PageModelName { get; set; }
        public DistrictViewModel Districts { get; set; }
    }

    public class City
    {
        public int CityNo { get; set; }
        public string CityName { get; set; }
        //public List<District> Districts { get; set; }
    }

    public class District
    {
        public int CityNo { get; set; }
        public int DistrictNo { get; set; }
        public string DistrictName { get; set; }
    }

    public class DistrictViewModel
    {
        public List<District> Districts { get; set; }
        public int SelectedDistrictNo { get; set; }
        public string PageModelName { get; set; }
    }
}
