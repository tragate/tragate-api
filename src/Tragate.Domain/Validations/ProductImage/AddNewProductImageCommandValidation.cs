using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.ProductImage
{
    public class AddNewProductImageCommandValidation : ProductImageValidation<AddNewProductImageCommand>
    {
        public AddNewProductImageCommandValidation(){
            ValidateUuId();
            ValidateFiles();
            ValidateFileCount();
        }
    }
}