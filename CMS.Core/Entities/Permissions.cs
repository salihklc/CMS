using AutoMapper;
using System.Collections.Generic;
using CMS.Core.Interfaces;
using CMS.Core.Mapping.Interface;
using CMS.Common.Models.ViewModels.Permission;

namespace CMS.Core.Entities
{
    public class Permissions :  BaseEntity, IAggregateRoot, IHaveCustomMapping
    {
        public int PermissionNo { get; set; }
        public string PermissionName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int PageIdx { get; set; }
        public Pages Page { get; set; }
        public ICollection<RolePermissions> RolePermissions { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Permission, Permissions>().ReverseMap();
        }
    }
}
