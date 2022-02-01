using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Common.Models.ViewModels.Permission;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Core.Interfaces;

namespace CMS.Core.Entities
{
    public class Roles : BaseEntity,IAggregateRoot
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public ICollection<RolePermissions> RolePermissions { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.AllowNullDestinationValues = true;
            configuration.CreateMap<RolePermissions, RolePermission>().ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permissions)).ReverseMap();
            configuration.CreateMap<Permissions, Permission>().ReverseMap();
            configuration.CreateMap<RolePermissionViewModel, Roles >().ReverseMap();
            configuration.CreateMap<Roles, CreateRoleModel>().ReverseMap();           
          
        }
    }
}