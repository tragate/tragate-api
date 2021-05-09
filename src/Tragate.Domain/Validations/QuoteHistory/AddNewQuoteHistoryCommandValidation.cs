using Tragate.Domain.Commands;
using Tragate.Domain.Validations.QuoteHistory;

namespace Tragate.Domain.Validations
{
    public class AddNewQuoteHistoryCommandValidation : QuoteHistoryValidation<AddNewQuoteHistoryCommand>
    {
        public AddNewQuoteHistoryCommandValidation(){
            ValidateQuoteId();
            ValidateDescription();
            ValidateCreatedUserId();
        }
    }
}