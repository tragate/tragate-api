using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdatedDateQuoteCommand : QuoteCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdatedDateQuoteCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}