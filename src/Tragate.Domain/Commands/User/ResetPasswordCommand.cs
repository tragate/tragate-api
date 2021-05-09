using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class ResetPasswordCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new ResetPasswordCommanValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}