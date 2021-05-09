using System;
using AutoMapper;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Events;
using Tragate.Domain.Models;

namespace Tragate.Application.AutoMapper
{
    public class DomainToEventMappingProfile : Profile
    {
        public DomainToEventMappingProfile(){
            CreateMap<User, UserRegisteredEvent>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FullName))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email));

            CreateMap<UserDto, UserForgotPasswordEvent>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email));


            // reverse event
            CreateMap<EmailSentEvent, Email>()
                .ForMember(x => x.To, y => y.Ignore())
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });
        }
    }
}