using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class DeleteCompanyTaskCommand : CompanyTaskCommand
    {
        public override bool IsValid(){
            ValidationResult = new DeleteCompanyTaskCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}