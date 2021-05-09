using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class AddNewCompanyTaskCommandValidation : CompanyTaskValidation<AddNewCompanyTaskCommand>
    {
        public AddNewCompanyTaskCommandValidation(){
            ValidateCompanyId();
            ValidateDescription();
            ValidateResponsibleUserId();
            ValidateCreatedUserId();
            ValidateCompanyTaskTypeId();
            ValidateStatusTypeId();
        }
    }
}