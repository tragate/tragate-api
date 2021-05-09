using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdateCompanyMembershipCommand : UserCompanyCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateCompanyMembershipCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}