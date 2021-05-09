using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class DeleteProductCommand : ProductCommand
    {
        public override bool IsValid(){
            ValidationResult = new DeleteProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}