using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class RegisterNewExternalUserCommand : UserCompanyCommand
    {
        public override bool IsValid(){
            ValidationResult = new RegisterNewExternalUserCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}