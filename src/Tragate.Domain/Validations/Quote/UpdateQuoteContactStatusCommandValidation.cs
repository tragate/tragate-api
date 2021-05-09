using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdateQuoteContactStatusCommandValidation : QuoteValidation<UpdateQuoteContactStatusCommand>
    {
        public UpdateQuoteContactStatusCommandValidation(){
            ValidateEmptyBuyerAndSellerContactStatusId();
            ValidateFillBuyerAndSellerContactStatusId();
        }
    }
}