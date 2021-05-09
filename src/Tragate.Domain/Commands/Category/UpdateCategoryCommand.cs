using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class UpdateCategoryCommand : CategoryCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new UpdateCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}