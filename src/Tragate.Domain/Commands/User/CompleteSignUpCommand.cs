using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class CompleteSignUpCommand : UserCompanyCommand
    {
        public override bool IsValid(){
            ValidationResult = new CompleteSignUpCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}