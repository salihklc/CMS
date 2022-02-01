using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CMS.Common.Models.ViewModels.Cities;
using CMS.Core.Interfaces;
using CMS.Core.Mapping.Interface;

namespace CMS.Core.Entities
{
    public class Cities : BaseEntity, IAggregateRoot,IHaveCustomMapping
    {
        public int CityNo { get; set; }
        public string CityName { get; set; }
        public ICollection<Districts> Districts { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.AllowNullCollections = true;
           
            configuration.CreateMap<Cities, City>().ReverseMap();
          
        }
    }
}
