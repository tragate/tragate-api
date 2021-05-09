using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class AddNewProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new AddNewProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}