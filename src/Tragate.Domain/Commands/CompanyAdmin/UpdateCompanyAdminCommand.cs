using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateCompanyAdminCommand : CompanyAdminCommand {
        public override bool IsValid () {
            ValidationResult = new UpdateCompanyAdminCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}