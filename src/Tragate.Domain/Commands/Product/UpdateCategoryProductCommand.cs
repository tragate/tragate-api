using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class UpdateCategoryProductCommand : ProductCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateCategoryProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}