using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Product
{
    public class AddNewProductCommandValidation : ProductValidation<AddNewProductCommand>
    {
        public AddNewProductCommandValidation(){
            ValidateCompanyId();
            ValidateCreatedUserId();
            ValidateTitle();
            ValidatePriceLow();
            ValidatePriceHigh();
            ValidateOriginLocationId();
            ValidateCategoryId();
            ValidateStatusId();
        }
    }
}