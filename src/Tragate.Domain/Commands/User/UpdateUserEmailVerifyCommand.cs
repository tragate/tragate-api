using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateUserEmailVerifyCommand : UserCompanyCommand {
        
        public override bool IsValid () {
            ValidationResult = new UpdateUserEmailVerifyCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}