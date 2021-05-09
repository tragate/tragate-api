using FluentValidation;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public abstract class CompanyAdminValidation<T> : AbstractValidator<T> where T : CompanyAdminCommand {
        protected void ValidateId () {
            RuleFor (c => c.Id)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Id");
        }

        protected void ValidateEmail () {
            RuleFor (c => c.Email)
                .NotEmpty ().WithMessage ("Please ensure you have entered the Email ")
                .EmailAddress ().WithMessage ("Please enter to valid email address");
        }

        protected void ValidateCompanyId () {
            RuleFor (c => c.CompanyId)
                .NotEmpty ().WithMessage ("Please ensure you have entered the CompanyId ");
        }

        protected void ValidateCompanyAdminRoleId () {
            RuleFor (c => c.CompanyAdminRoleId)
                .NotEmpty ().WithMessage ("Please ensure you have entered the CompanyAdminRoleId ");
        }

        protected void ValidateStatusId () {
            RuleFor (c => c)
                .Custom ((c, context) => {
                    if (c.StatusType == StatusType.All) {
                        context.AddFailure ("StatusType", "Status shouldn't be 0");
                    }
                });
        }
    }
}