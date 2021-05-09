using Tragate.Domain.Validations.ProductImage;

namespace Tragate.Domain.Commands
{
    public class AddNewProductImageCommand : ProductImageCommand
    {
        public override bool IsValid(){
            ValidationResult = new AddNewProductImageCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}