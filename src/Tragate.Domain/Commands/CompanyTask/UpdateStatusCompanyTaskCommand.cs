using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdateStatusCompanyTaskCommand : CompanyTaskCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateStatusCompanyTaskCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}