using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class ChangeEmailCommand : UserCompanyCommand
    {
        public override bool IsValid(){
            ValidationResult = new ChangeEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}