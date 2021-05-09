using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class AddNewQuoteHistoryCommand : QuoteHistoryCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewQuoteHistoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}