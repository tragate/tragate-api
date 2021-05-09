using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateCompanyAdminCommandValidation : CompanyAdminValidation<UpdateCompanyAdminCommand> {
        public UpdateCompanyAdminCommandValidation () {
            ValidateId ();
            ValidateEmail ();
            ValidateCompanyId ();
            ValidateCompanyAdminRoleId ();
            ValidateStatusId();
        }
    }
}