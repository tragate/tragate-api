using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class RegisterNewPersonCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new RegisterNewPersonCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}