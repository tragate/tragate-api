using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class AddNewQuoteCommandValidation : QuoteValidation<AddNewQuoteCommand>
    {
        public AddNewQuoteCommandValidation(){
            ValidateTitle();
            ValidateDescription();
            ValidateBuyerUserId();
            ValidateSellerCompanyId();
            ValidateCreatedUserId();
            ValidateBuyerUserEmail();
        }
    }
}