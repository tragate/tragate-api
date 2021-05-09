using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class RemoveCompanyAdminCommandValidation : CompanyAdminValidation<RemoveCompanyAdminCommand> {
        public RemoveCompanyAdminCommandValidation () {
            ValidateId ();
        }
    }
}