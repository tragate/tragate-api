using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public abstract class MailValidation<T> : AbstractValidator<T> where T : MailCommand
    {
        protected void ValidateMailName(){
            RuleFor(c => c.MailName).NotEmpty();
        }

        protected void ValidateMailTitle(){
            RuleFor(c => c.MailTitle).NotEmpty();
        }

        protected void ValidateMailList(){
            RuleFor(c => c.To)
                .NotEmpty();
        }
    }
}