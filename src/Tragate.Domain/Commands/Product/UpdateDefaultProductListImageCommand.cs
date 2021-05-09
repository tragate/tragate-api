using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class UpdateDefaultProductListImageCommand : ProductCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateDefaultProductListImageCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}