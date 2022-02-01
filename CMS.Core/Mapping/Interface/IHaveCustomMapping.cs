using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Core.Mapping.Interface
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
