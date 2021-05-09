using System;
using AutoMapper;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Models;

namespace Tragate.Application.AutoMapper
{
    public class DomainToDomainMappingProfile : Profile
    {
        public DomainToDomainMappingProfile(){
            CreateMap<User, CompanyAdmin>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CompanyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.CompanyAdminRoleId, opt => opt.UseValue(1))
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });


            CreateMap<ProductDto, ProductDetailDto>()
                .ForMember(x => x.ProductTitle, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.ProductSlug, opt => opt.MapFrom(x => x.Slug))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath.CheckProductProfileImage()))
                .ForMember(x => x.CompanyTitle, opt => opt.MapFrom(x => x.Company.User.FullName))
                .ForMember(x => x.CompanySlug, opt => opt.MapFrom(x => x.Company.Slug))
                .ForMember(x => x.CompanyStatusId, opt => opt.MapFrom(x => x.Company.StatusId))
                .ForMember(x => x.Location, opt => opt.MapFrom(x => x.Company.User.Location.Name))
                .ForMember(x => x.EstablishmentYear, opt => opt.MapFrom(x => x.Company.EstablishmentYear))
                .ForMember(x => x.MembershipTypeId, opt => opt.MapFrom(x => x.Company.MembershipTypeId))
                .ForMember(x => x.MembershipType, opt => opt.MapFrom(x => x.Company.MembershipType))
                .ForMember(x => x.VerificationTypeId, opt => opt.MapFrom(x => x.Company.VerificationTypeId))
                .ForMember(x => x.VerificationType, opt => opt.MapFrom(x => x.Company.VerificationType))
                .ForMember(x => x.TransactionCount, opt => opt.MapFrom(x => x.Company.TransactionCount))
                .ForMember(x => x.TransactionAmount, opt => opt.MapFrom(x => x.Company.TransactionAmount))
                .ForMember(x => x.ResponseRate, opt => opt.MapFrom(x => x.Company.ResponseRate))
                .ForMember(x => x.ResponseTime, opt => opt.MapFrom(x => x.Company.ResponseTime));

            CreateMap<ProductDto, ProductListDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ProductTitle, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.ProductSlug, opt => opt.MapFrom(x => x.Slug))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath))
                .ForMember(x => x.CompanyTitle, opt => opt.MapFrom(x => x.Company.User.FullName))
                .ForMember(x => x.CompanySlug, opt => opt.MapFrom(x => x.Company.Slug))
                .ForMember(x => x.MembershipType, opt => opt.MapFrom(x => x.Company.MembershipType));

            CreateMap<ProductDto, CompanyProductDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ProductTitle, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.ProductSlug, opt => opt.MapFrom(x => x.Slug))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath))
                .ForMember(x => x.CompanyTitle, opt => opt.MapFrom(x => x.Company.User.FullName))
                .ForMember(x => x.CompanySlug, opt => opt.MapFrom(x => x.Company.Slug))
                .ForMember(x => x.MembershipType, opt => opt.MapFrom(x => x.Company.MembershipType));

            CreateMap<ProductDto, UserProductDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ProductTitle, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.ProductSlug, opt => opt.MapFrom(x => x.Slug))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath))
                .ForMember(x => x.CompanyTitle, opt => opt.MapFrom(x => x.Company.User.FullName))
                .ForMember(x => x.CompanySlug, opt => opt.MapFrom(x => x.Company.Slug))
                .ForMember(x => x.MembershipType, opt => opt.MapFrom(x => x.Company.MembershipType));

            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<CompanyTaskDto, UserTaskDto>();
        }
    }
}