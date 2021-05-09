using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class AddNewCompanyAdminCommand : CompanyAdminCommand {
        public override bool IsValid () {
            ValidationResult = new AddNewCompanyAdminCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}