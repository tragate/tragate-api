using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class AddNewCompanyNoteCommand : CompanyNoteCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewCompanyNoteCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}