using Tragate.Domain.Validations.Product;

namespace Tragate.Domain.Commands
{
    public class UpdateStatusProductCommand : ProductCommand
    {
        public override bool IsValid(){
            ValidationResult = new UpdateStatusProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}