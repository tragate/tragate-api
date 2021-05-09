using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateCompanyDataCommandValidation : CompanyDataValidation<UpdateCompanyDataCommand> {
        public UpdateCompanyDataCommandValidation () {
            ValidateId ();
            ValidateStatusId ();
            ValidateTitle ();
            ValidateCompanyProfileLink ();
        }
    }
}