using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class AddNewContentCommand : ContentCommand {
        public override bool IsValid () {
            ValidationResult = new AddNewContentCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}