using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class RegisterNewCompanyCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new RegisterNewCompanyCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}