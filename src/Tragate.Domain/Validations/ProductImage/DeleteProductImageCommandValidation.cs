using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.ProductImage
{
    public class DeleteProductImageCommandValidation : ProductImageValidation<DeleteProductImageCommand>
    {
        public DeleteProductImageCommandValidation(){
            ValidateId();
        }
    }
}