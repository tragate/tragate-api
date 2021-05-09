using FluentValidation;
using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.QuoteHistory
{
    public abstract class QuoteHistoryValidation<T> : AbstractValidator<T> where T : QuoteHistoryCommand
    {
        protected void ValidateId(){
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateQuoteId(){
            RuleFor(c => c.QuoteId)
                .NotEmpty()
                .GreaterThan(0);
        }

        protected void ValidateDescription(){
            RuleFor(c => c.Description)
                .NotEmpty();
        }

        protected void ValidateCreatedUserId(){
            RuleFor(c => c.CreatedUserId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}