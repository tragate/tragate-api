using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateCompanyCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new UpdateCompanyCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}