using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CMS.Core.Mapping.Interface;

namespace CMS.Core.Mapping.Infrastructure
{
    public class Map
    {
        public Type Source { get; set; }
        public Type Destination { get; set; }
    }

    public static class MapperProfileHelper
    {
        public static IList<Map> LoadStandardMappings(Assembly rootAssembly)
        {
            var types = rootAssembly.GetExportedTypes();

            var mapsFrom = (
                    from type in types
                    from instance in type.GetInterfaces()
                    where
                        instance.IsGenericType && instance.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                        !type.IsAbstract &&
                        !type.IsInterface
                    select new Map
                    {
                        Source = type.GetInterfaces().First().GetGenericArguments().First(),
                        Destination = type
                    }).ToList();

            return mapsFrom;
        }

        public static IList<IHaveCustomMapping> LoadCustomMappings(Assembly rootAssembly)
        {
            var types = rootAssembly.GetExportedTypes();

            var mapsFrom = (
                    from type in types
                    from instance in type.GetInterfaces()
                    where
                        typeof(IHaveCustomMapping).IsAssignableFrom(type) &&
                        !type.IsAbstract &&
                        !type.IsInterface
                    select (IHaveCustomMapping)Activator.CreateInstance(type)).ToList();

            return mapsFrom;
        }


        public static Dictionary<string, string> GetDestinationPropertyFor<TSrc, TDst>(Mapper mapper, List<string> sourceProperties)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var map = mapper.DefaultContext.ConfigurationProvider.FindTypeMapFor<TSrc, TDst>();
            // var destinationList = map.PropertyMaps.Where(f => sourceProperties.Contains(f.SourceMember.Name)).Select(f => f.DestinationName).ToList();

            foreach (var item in map.PropertyMaps)
            {
                if (item.SourceMember != null && sourceProperties.Contains(item.SourceMember.Name))
                {
                    result.Add(item.SourceMember.Name, item.DestinationMember.Name);
                }
            }

            return result;
        }
    }
}
