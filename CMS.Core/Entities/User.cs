using System;
using System.Collections.Generic;
using AutoMapper;
using CMS.Common.Models.ViewModels.Roles;
using CMS.Common.Models.ViewModels.Users;
using CMS.Core.Interfaces;
using CMS.Core.Mapping.Interface;

namespace CMS.Core.Entities
{
    public class User : BaseEntity,IAggregateRoot, IHaveCustomMapping
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }    
        public string Email { get; set; }      
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string Address { get; set; }
        public int? CityNo { get; set; }
        public int? DistrictNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastSuccessLogin { get; set; }
        public int? DepartmanIdx { get; set; }
        public int? JobIdx { get; set; }
        public string DefaultLanguage { get; set; }
        public string Tags { get; set; }
        public string Picture { get; set; }
        public string PictureThumb { get; set; }
        public int? GroupIdx { get; set; }
        public int State { get; set; }
        public int? FirmIdx { get; set; }
        public Firms Firms { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection<TicketWatchers> TicketWatchers { get; set; }
        public ICollection<TicketComments> UserTicketComments { get; set; }
        public ICollection<TicketLogTimes> TicketLogTimes { get; set; }
        public ICollection<TicketHistories> TicketHistories { get; set; }
        public ICollection<Tickets> Tickets { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateUserModel, User>().ReverseMap();
            configuration.CreateMap<UpdateUserModel, User>().ReverseMap();
            configuration.CreateMap<UserViewModel, User>().ForMember(dest=> dest.UserRoles,opt => opt.MapFrom(src=> src.UserRoles)).ReverseMap();
            configuration.CreateMap<UserRolesViewModel, UserRoles>().ForMember(dest=> dest.Roles,opt=> opt.MapFrom(src => src.Roles)).ReverseMap();
            configuration.CreateMap<RoleViewModel, Roles>().ReverseMap();
        }
    }
}