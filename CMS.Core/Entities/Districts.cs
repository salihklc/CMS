using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Core.Interfaces;
using CMS.Core.Mapping.Interface;

namespace CMS.Core.Entities
{
    public class Districts : BaseEntity, IAggregateRoot, IHaveCustomMapping
    {
        public int DistrictNo { get; set; }
        public string DistrictName { get; set; }
        public int CityNo { get; set; }
        public Cities City { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<District, Districts>().ReverseMap();
        }
    }
}
