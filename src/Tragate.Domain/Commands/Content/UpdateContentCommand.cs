using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UpdateContentCommand : ContentCommand {
        public override bool IsValid () {
            ValidationResult = new UpdateContentCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}