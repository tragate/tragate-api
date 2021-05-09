using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class DeleteCompanyNoteCommandValidation : CompanyNoteValidation<CompanyNoteCommand>
    {
        public DeleteCompanyNoteCommandValidation(){
            ValidateId();
        }
    }
}