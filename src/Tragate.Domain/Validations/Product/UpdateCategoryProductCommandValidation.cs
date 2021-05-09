using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class UpdateCategoryProductCommandValidation : ProductValidation<UpdateCategoryProductCommand>
    {
        public UpdateCategoryProductCommandValidation(){
            ValidateCategoryId();
        }
    }
}