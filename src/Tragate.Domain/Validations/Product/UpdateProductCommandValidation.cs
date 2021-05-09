using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class UpdateProductCommandValidation : ProductValidation<UpdateProductCommand>
    {
        public UpdateProductCommandValidation(){
            ValidateId();
            ValidateCompanyId();
            ValidateUpdatedUserId();
            ValidateTitle();
            ValidatePriceLow();
            ValidatePriceHigh();
            ValidateOriginLocationId();
            ValidateCategoryId();
            ValidateStatusId();
        }
    }
}