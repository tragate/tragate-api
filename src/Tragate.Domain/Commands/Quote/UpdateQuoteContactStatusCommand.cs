using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdateQuoteContactStatusCommand : QuoteCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateQuoteContactStatusCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}