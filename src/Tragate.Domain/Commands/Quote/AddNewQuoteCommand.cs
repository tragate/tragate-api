using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class AddNewQuoteCommand : QuoteCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewQuoteCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}