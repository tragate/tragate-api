using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class UpdateDefaultProductListImageCommandValidation : ProductValidation<UpdateDefaultProductListImageCommand>
    {
        public UpdateDefaultProductListImageCommandValidation(){
            ValidateId();
            ValidateUpdatedUserId();
            ValidateListImagePath();
        }
    }
}