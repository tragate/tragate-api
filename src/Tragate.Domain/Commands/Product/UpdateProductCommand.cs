using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class UpdateProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new UpdateProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}