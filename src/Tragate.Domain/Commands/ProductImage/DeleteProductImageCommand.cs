using Tragate.Domain.Validations.ProductImage;

namespace Tragate.Domain.Commands
{
    public class DeleteProductImageCommand : ProductImageCommand
    {
        public override bool IsValid(){
            ValidationResult = new DeleteProductImageCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}