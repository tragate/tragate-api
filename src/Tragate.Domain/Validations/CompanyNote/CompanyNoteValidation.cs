using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class CompanyNoteValidation<T> : AbstractValidator<T> where T : CompanyNoteCommand
    {
        public void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0);
        }

        public void ValidateCompanyId(){
            RuleFor(c => c.CompanyId)
                .NotEmpty();
        }

        public void ValidateDescription(){
            RuleFor(c => c.Description)
                .NotEmpty();
        }

        public void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}