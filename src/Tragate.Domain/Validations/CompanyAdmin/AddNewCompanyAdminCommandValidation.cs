using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class AddNewCompanyAdminCommandValidation : CompanyAdminValidation<AddNewCompanyAdminCommand> {
        public AddNewCompanyAdminCommandValidation () {
            ValidateEmail ();
            ValidateCompanyId ();
            ValidateCompanyAdminRoleId ();
            ValidateStatusId();
        }
    }
}