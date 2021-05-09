using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class SendActivationEmailCommand : UserCompanyCommand
    {
        public override bool IsValid(){
            ValidationResult = new SendActivationEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}