using Nest;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Profile = AutoMapper.Profile;

namespace Tragate.Application.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public ModelToDomainMappingProfile(){
            CreateMap<PersonViewModel, RegisterNewPersonCommand>();

            CreateMap<PersonViewModel, UpdateUserCommand>();

            CreateMap<CompanyViewModel, RegisterNewCompanyCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId))
                .ForMember(x => x.CompanyCategoryIds, opt => opt.MapFrom(x => x.CategoryIds));

            CreateMap<CompanyFastAddViewModel, RegisterFastNewCompanyCommand>()
                .ForMember(x => x.LocationId, opt => opt.MapFrom(x => x.CityId))
                .ForMember(x => x.StateId, opt => opt.MapFrom(x => x.CityId))
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => StatusType.Active))
                .ForMember(x => x.CompanyCategoryIds, opt => opt.MapFrom(x => new int[] { x.CategoryId }));

            CreateMap<CompanyViewModel, UpdateCompanyCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId))
                .ForMember(x => x.CompanyCategoryIds, opt => opt.MapFrom(x => x.CategoryIds));

            CreateMap<CategoryViewModel, AddNewCategoryCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<CategoryViewModel, UpdateCategoryCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<CompanyDataViewModel, UpdateCompanyDataCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<ForgotPasswordViewModel, ForgotPasswordCommand>();

            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();

            CreateMap<UserChangePasswordViewModel, ChangePasswordCommand>();

            CreateMap<CompanyAdminViewModel, AddNewCompanyAdminCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<CompanyAdminViewModel, UpdateCompanyAdminCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<ContentViewModel, AddNewContentCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<ContentViewModel, UpdateContentCommand>()
                .ForMember(x => x.StatusType, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<ProductViewModel, AddNewProductCommand>();

            CreateMap<ProductViewModel, UpdateProductCommand>();

            CreateMap<ProductStatusViewModel, UpdateStatusProductCommand>();

            CreateMap<ProductCategoryViewModel, UpdateCategoryProductCommand>();

            CreateMap<UserChangeEmailViewModel, ChangeEmailCommand>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.Password, opt => opt.MapFrom(x => x.Password))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<CompanyMembershipViewModel, AddNewCompanyMembershipCommand>();
            CreateMap<CompanyNoteViewModel, AddNewCompanyNoteCommand>();

            CreateMap<CompanyTaskViewModel, AddNewCompanyTaskCommand>()
                .ForMember(x => x.CompanyTaskTypeId, opt => opt.MapFrom(x => (CompanyTaskType) x.CompanyTaskTypeId))
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (StatusType) x.StatusId));

            CreateMap<CompanyTaskStatusViewModel, UpdateStatusCompanyTaskCommand>()
                .ForMember(x => x.StatusId, opt => opt.MapFrom(x => (StatusType) x.StatusId))
                .ForMember(x => x.UpdatedUserId, opt => opt.MapFrom(x => x.UpdatedUserId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<CreateQuoteViewModel, AddNewQuoteCommand>()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.UserMessage));

            CreateMap<CreateQuoteHistoryViewModel, AddNewQuoteHistoryCommand>();
            CreateMap<CompleteSignUpViewModel, CompleteSignUpCommand>();
            CreateMap<ExternalSignUpViewModel, RegisterNewExternalUserCommand>()
                .ForMember(x => x.RegisterTypeId, opt => opt.MapFrom(x => x.RegisterTypeId));
        }
    }
}