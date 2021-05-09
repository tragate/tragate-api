using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class RemoveCompanyCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new RemoveCompanyCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}