using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class AddNewCompanyTaskCommand : CompanyTaskCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewCompanyTaskCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}