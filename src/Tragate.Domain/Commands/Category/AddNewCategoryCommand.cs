using System;
using Tragate.Domain.Validations;

namespace Tragate.Domain.Commands
{
    public class AddNewCategoryCommand : CategoryCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new AddNewCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}