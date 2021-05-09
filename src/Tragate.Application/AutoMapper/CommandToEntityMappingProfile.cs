using System;
using AutoMapper;
using Tragate.Common.Library;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Commands;
using Tragate.Domain.Models;

namespace Tragate.Application.AutoMapper
{
    public class CommandToEntityMappingProfile : Profile
    {
        public CommandToEntityMappingProfile(){
            CreateMap<RegisterNewPersonCommand, User>()
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(false))
                .AfterMap((src, dest) =>
                {
                    var salt = PasswordHashHelper.GetSalt();
                    dest.Password = PasswordHashHelper.HashPassword(src.Password, salt);
                    dest.Salt = salt;
                    dest.CreatedDate = DateTime.Now;
                });

            CreateMap<RegisterNewCompanyCommand, User>()
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(2))
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(false))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<RegisterFastNewCompanyCommand,User>()
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(false))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<RegisterNewCompanyCommand, Company>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.FullName.GenerateSlug()))
                .ForMember(x => x.MembershipTypeId, opt => opt.UseValue(0))
                .ForMember(x => x.MembershipPackageId, opt => opt.UseValue(0))
                .ForMember(x => x.VerificationTypeId, opt => opt.UseValue(0))
                .ForMember(x => x.TransactionCount, opt => opt.UseValue(0))
                .ForMember(x => x.TransactionAmount, opt => opt.UseValue(0))
                .ForMember(x => x.ResponseTime, opt => opt.UseValue(0))
                .ForMember(x => x.ResponseRate, opt => opt.UseValue(0))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; });

            CreateMap<UpdateCompanyCommand, User>()
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(2))
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(false))
                .ForMember(x => x.ProfileImagePath, opt => opt.Ignore());

            CreateMap<UpdateCompanyCommand, Company>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.MembershipTypeId, opt => opt.Ignore())
                .ForMember(x => x.MembershipPackageId, opt => opt.Ignore())
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.FullName.GenerateSlug()))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; });

            CreateMap<UpdateCompanyMembershipCommand, Company>()
                .ForMember(x => x.MembershipTypeId, opt => opt.MapFrom(x => x.MembershipTypeId))
                .ForMember(x => x.MembershipPackageId, opt => opt.MapFrom(x => x.MembershipPackageId))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());


            CreateMap<RemoveCompanyCommand, User>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FullName))
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(2))
                .ForMember(x => x.StatusId, opt => opt.UseValue(4))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(false));

            CreateMap<AddNewCategoryCommand, Category>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.ParentId, opt => opt.MapFrom(x => x.ParentId))
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.MetaKeyword, opt => opt.MapFrom(x => x.Metakeyword))
                .ForMember(x => x.MetaDescription, opt => opt.MapFrom(x => x.MetaDescription))
                .ForMember(x => x.CreatedUserId, opt => opt.MapFrom(x => x.CreatedUserId))
                .AfterMap((src, dest) =>
                {
                    dest.Slug = src.Title.GenerateSlug();
                    dest.CreatedDate = DateTime.Now;
                });

            CreateMap<UpdateCategoryCommand, Category>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.Slug = src.Title.GenerateSlug(); });

            CreateMap<UpdateReferenceCompanyDataCommand, CompanyData>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.CompanyId, opt => opt.MapFrom(x => x.CompanyId))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UpdateCompanyDataCommand, CompanyData>()
                .ForMember(x => x.ProfileImagePath, opt => opt.Ignore());

            CreateMap<UpdateUserCommand, User>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FullName))
                .ForMember(x => x.LanguageId, opt => opt.MapFrom(x => x.LanguageId))
                .ForMember(x => x.TimezoneId, opt => opt.MapFrom(x => x.TimezoneId))
                .ForMember(x => x.LocationId, opt => opt.MapFrom(x => x.LocationId))
                .ForMember(x => x.CountryId, opt => opt.MapFrom(x => x.CountryId))
                .ForMember(x => x.StateId, opt => opt.MapFrom(x => x.StateId))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<AddNewCompanyAdminCommand, CompanyAdmin>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<UpdateCompanyAdminCommand, CompanyAdmin>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.CompanyId, opt => opt.Ignore());

            CreateMap<AddNewContentCommand, Content>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Title.GenerateSlug()))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<UpdateContentCommand, Content>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusType))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Title.GenerateSlug()));

            CreateMap<AddNewProductCommand, Product>()
                .ForMember(x => x.UpdatedUserId, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(x => x.CreatedUserId, opt => opt.Ignore())
                .ForMember(x => x.UuId, opt => opt.Ignore())
                .ForMember(x => x.ListImagePath, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Slug = $"{src.Title.GenerateSlug()}-{src.Id}";
                    dest.UpdatedDate = DateTime.Now;
                });

            CreateMap<UpdateStatusProductCommand, Product>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => x.StatusId))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UpdateDefaultProductListImageCommand, Product>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UpdateProductListImageCommand, Product>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .ForMember(x => x.ListImagePath, opt => opt.MapFrom(x => x.ListImagePath))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UpdateCategoryProductCommand, Product>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.CategoryId))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .AfterMap((src, dest) => { dest.UpdatedDate = DateTime.Now; })
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<AddNewCompanyMembershipCommand, CompanyMembership>()
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<AddNewCompanyNoteCommand, CompanyNote>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.StatusId, opt => opt.UseValue((int) StatusType.Active))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<AddNewCompanyTaskCommand, CompanyTask>()
                .ForMember(x => x.UpdatedUserId, opt => opt.Ignore())
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (byte) x.StatusId))
                .ForMember(x => x.CompanyTaskTypeId, opt => opt.MapFrom(x => (byte) x.CompanyTaskTypeId))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<AddNewQuoteCommand, User>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.BuyerUserEmail))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.BuyerUserEmail))
                .ForMember(x => x.StateId, opt => opt.MapFrom(x => x.BuyerUserStateId))
                .ForMember(x => x.LocationId, opt => opt.MapFrom(x => x.BuyerUserStateId))
                .ForMember(x => x.CountryId, opt => opt.MapFrom(x => x.BuyerUserCountryId))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue((int) UserType.Person))
                .ForMember(x => x.RegisterTypeId, opt => opt.UseValue((int) RegisterType.Anonymous))
                .ForMember(x => x.StatusId, opt => opt.UseValue((int) StatusType.Active))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<AddNewQuoteCommand, Quote>()
                .ForMember(x => x.QuoteStatusId, opt => opt.UseValue((int) QuoteStatusType.Lead))
                .ForMember(x => x.OrderStatusId, opt => opt.UseValue((int) OrderStatusType.Waiting_Price))
                .ForMember(x => x.BuyerContactStatusId,
                    opt => opt.UseValue((int) QuoteContactStatusType.Buyer_Read))
                .ForMember(x => x.SellerContactStatusId,
                    opt => opt.UseValue((int) QuoteContactStatusType.Waiting_Seller_Response))
                .AfterMap((src, dest) =>
                {
                    dest.CreatedDate = DateTime.Now;
                    dest.UpdatedDate = DateTime.Now;
                });

            CreateMap<AddNewQuoteCommand, QuoteProduct>()
                .ForMember(x => x.Quantity, opt => opt.MapFrom(x => x.ProductQuantity))
                .ForMember(x => x.UnitTypeId, opt => opt.MapFrom(x => x.ProductUnitTypeId))
                .ForMember(x => x.Note, opt => opt.MapFrom(x => x.ProductNote))
                .ForMember(x => x.Note, opt => opt.MapFrom(x => x.ProductNote))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<AddNewQuoteHistoryCommand, QuoteHistory>()
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });

            CreateMap<RegisterNewExternalUserCommand, User>()
                .ForMember(x => x.StatusId, opt => opt.UseValue(3))
                .ForMember(x => x.UserTypeId, opt => opt.UseValue(1))
                .ForMember(x => x.EmailVerified, opt => opt.UseValue(true))
                .ForMember(x => x.LocationId, opt => opt.MapFrom(x => x.StateId))
                .AfterMap((src, dest) => { dest.CreatedDate = DateTime.Now; });


            //Entity to Command - Reverse Mapping

            CreateMap<Quote, AddNewQuoteHistoryCommand>()
                .ForMember(x => x.QuoteId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}