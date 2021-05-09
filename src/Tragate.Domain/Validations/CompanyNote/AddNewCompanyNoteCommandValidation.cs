using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class AddNewCompanyNoteCommandValidation : CompanyNoteValidation<AddNewCompanyNoteCommand>
    {
        public AddNewCompanyNoteCommandValidation(){
            ValidateCompanyId();
            ValidateDescription();
            ValidateCreatedUserId();
        }
    }
}