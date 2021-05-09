using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public abstract class UserValidation<T> : AbstractValidator<T> where T : UserCompanyCommand
    {
        protected void ValidateExternalUserId(){
            RuleFor(c => c.ExternalUserId)
                .NotEmpty();
        }

        protected void ValidateEmail(){
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Please ensure you have entered the Email ")
                .EmailAddress().WithMessage("Please enter to valid email address");
        }

        protected void ValidateProfileImagePath(){
            RuleFor(c => c.ProfileImagePath).NotEmpty();
        }

        protected void ValidatePasswordMatch(){
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the Password");

            RuleFor(c => c.PasswordMatch)
                .NotEmpty().WithMessage("Please ensure you have entered the PasswordMatch");

            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (!(string.IsNullOrEmpty(c.Password) && string.IsNullOrEmpty(c.PasswordMatch))){
                        if (!c.Password.Equals(c.PasswordMatch)){
                            context.AddFailure("Password", "Doesn't match password");
                        }
                    }
                });
        }

        protected void ValidatePassword(){
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the Password");
        }

        protected void ValidateFullName(){
            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("Please ensure you have entered the UserFullName");
        }

        protected void ValidatePerson(){
            RuleFor(c => c.PersonId)
                .NotEmpty().WithMessage("Please ensure you have entered the PersonId");
        }

        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Please ensure you have entered the Id");
        }

        protected void ValidateStatusId(){
            RuleFor(c => c)
                .Custom((c, context) =>
                {
                    if (c.StatusType == StatusType.All){
                        context.AddFailure("StatusType", "Status shouldn't be 0");
                    }
                });
        }

        protected void ValidateRegisterType(){
            RuleFor(c => c.RegisterTypeId)
                .NotEmpty()
                .Must(x =>
                    x.Equals(RegisterType.Facebook) ||
                    x.Equals(RegisterType.Google) ||
                    x.Equals(RegisterType.Linkedin) ||
                    x.Equals(RegisterType.Twitter));
        }

        protected void ValidateBusinessType(){
            RuleFor(c => c.BusinessType)
                .NotEmpty().WithMessage("Please ensure you have entered the BusinessType");
        }

        protected void ValidateEmployeeCountId(){
            RuleFor(c => c.EmployeeCountId)
                .NotEmpty().WithMessage("Please ensure you have entered the EmployeeCountId");
        }

        protected void ValidateAnnualRenevueId(){
            RuleFor(c => c.AnnualRevenueId)
                .NotEmpty().WithMessage("Please ensure you have entered the AnnualRevenueId");
        }

        protected void ValidateLocationId(){
            RuleFor(c => c.LocationId)
                .NotEmpty().WithMessage("Please ensure you have entered the LocationId");
        }

        protected void ValidateCountryId(){
            RuleFor(c => c.CountryId)
                .NotEmpty().WithMessage("Please ensure you have entered the CountryId");
        }

        protected void ValidateStateId(){
            RuleFor(c => c.StateId)
                .NotEmpty().WithMessage("Please ensure you have entered the StateId");
        }

        protected void ValidateLanguageId(){
            RuleFor(c => c.LanguageId)
                .NotEmpty().WithMessage("Please ensure you have entered the LanguageId");
        }

        protected void ValidateTimezoneId(){
            RuleFor(c => c.TimezoneId)
                .NotEmpty().WithMessage("Please ensure you have entered the TimezoneId");
        }


        protected void ValidateMembershipTypeId(){
            RuleFor(c => c.MembershipTypeId)
                .GreaterThanOrEqualTo(0);
        }

        protected void ValidateMembershipPackageId(){
            RuleFor(c => c.MembershipPackageId)
                .GreaterThanOrEqualTo(0);
        }

        protected void ValidatePhone()
        {
            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Please ensure you have entered the Phone");
        }
    }
}