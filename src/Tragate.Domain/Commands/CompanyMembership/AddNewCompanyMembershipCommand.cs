using Tragate.Domain.Validations.CompanyMembership;

namespace Tragate.Domain.Commands
{
    public class AddNewCompanyMembershipCommand : CompanyMembershipCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewCompanyMembershipCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}