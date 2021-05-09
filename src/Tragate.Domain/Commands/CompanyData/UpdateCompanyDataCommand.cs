using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateCompanyDataCommand : CompanyDataCommand {
        public override bool IsValid () {
            ValidationResult = new UpdateCompanyDataCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}