using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class DeleteCompanyTaskCommandValidation : CompanyTaskValidation<DeleteCompanyTaskCommand>
    {
        public DeleteCompanyTaskCommandValidation(){
            ValidateId();
        }
    }
}