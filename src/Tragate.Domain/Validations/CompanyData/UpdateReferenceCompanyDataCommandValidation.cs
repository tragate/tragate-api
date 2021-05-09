using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdateReferenceCompanyDataCommandValidation : CompanyDataValidation<UpdateReferenceCompanyDataCommand>
    {
        public UpdateReferenceCompanyDataCommandValidation(){
            ValidateId();
            ValidateStatusId();
            ValidateUpdatedUserId();
        }
    }
}