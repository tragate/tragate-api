using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class DeleteCompanyNoteCommand : CompanyNoteCommand
    {
        public override bool IsValid(){
            ValidationResult = new DeleteCompanyNoteCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}