using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdateStatusCompanyTaskCommandValidation : CompanyTaskValidation<UpdateStatusCompanyTaskCommand>
    {
        public UpdateStatusCompanyTaskCommandValidation(){
            ValidateId();
            ValidateStatusTypeId();
            ValidateUpdatedUserId();
        }
    }
}