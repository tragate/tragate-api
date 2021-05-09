using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdateReferenceCompanyDataCommand : CompanyDataCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateReferenceCompanyDataCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}