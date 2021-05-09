using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Models;

namespace Tragate.Application.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public DomainToModelMappingProfile(){
            CreateMap<User, PersonViewModel>();
            CreateMap<User, CompanyViewModel>();

            CreateMap<User, UserDto>()
                .ForMember(x => x.UserType, opt => opt.MapFrom(x => (UserType) x.UserTypeId))
                .ForMember(x => x.RegisterType, opt => opt.MapFrom(x => (RegisterType) x.RegisterTypeId))
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId))
                .ForMember(x => x.ProfileImagePath,
                    opt => opt.MapFrom(x => x.ProfileImagePath.CheckUserProfileImage()));

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.ImagePath, opt => opt.MapFrom(x => x.ImagePath.CheckCategoryProfileImage()));

            CreateMap<Location, LocationDto>();
            CreateMap<CompanyData, CompanyDataDto>();
            CreateMap<Parameter, ParameterDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => (int) x.ParameterCode))
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.ParameterValue1));
            CreateMap<Content, ContentDto>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<CompanyData, CompanyDataSearchDto>();
            CreateMap<User, QuoteUserDto>();
        }
    }
}