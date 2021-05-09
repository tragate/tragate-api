using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands {
    public class UploadCategoryImageCommand : CategoryCommand {
        public override bool IsValid () {
            ValidationResult = new UploadCategoryImageCommandValidation ().Validate (this);
            return ValidationResult.IsValid;
        }
    }
}