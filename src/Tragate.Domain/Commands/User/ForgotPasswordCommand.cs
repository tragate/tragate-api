using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class ForgotPasswordCommand : UserCompanyCommand {
        public override bool IsValid () {
            ValidationResult = new ForgotPasswordCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}