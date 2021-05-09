using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class UpdatedDateQuoteCommandValidation : QuoteValidation<UpdatedDateQuoteCommand>
    {
        public UpdatedDateQuoteCommandValidation(){
            ValidateId();
        }
    }
}