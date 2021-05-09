using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class DeleteProductCommandValidation : ProductValidation<DeleteProductCommand>
    {
        public DeleteProductCommandValidation(){
            ValidateId();
        }
    }
}