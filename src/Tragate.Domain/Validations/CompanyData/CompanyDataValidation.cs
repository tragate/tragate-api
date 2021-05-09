using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public abstract class CompanyDataValidation<T> : AbstractValidator<T> where T : CompanyDataCommand
    {
        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Please ensure you have entered the Id ");
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

        protected void ValidateUpdatedUserId(){
            RuleFor(c => c.UpdatedUserId)
                .NotEmpty().WithMessage("Please ensure you have entered the UpdatedUserId")
                .GreaterThan(0).WithMessage("UpdatedUserId should be greater than 0");
        }

        protected void ValidateTitle(){
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Please ensure you have entered the Title ");
        }

        protected void ValidateCompanyProfileLink(){
            RuleFor(c => c.CompanyProfileLink)
                .NotEmpty().WithMessage("Please ensure you have entered the CompanyProfileLink ");
        }

        protected void ValidateCompanyId(){
            RuleFor(c => c.CompanyId)
                .NotEmpty().WithMessage("Please ensure you have entered the CompanyId");
        }
    }
}