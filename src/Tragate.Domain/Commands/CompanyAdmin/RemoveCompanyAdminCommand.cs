using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class RemoveCompanyAdminCommand : CompanyAdminCommand {
        public override bool IsValid () {
            ValidationResult = new RemoveCompanyAdminCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}