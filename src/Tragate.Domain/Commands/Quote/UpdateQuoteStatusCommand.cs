namespace Tragate.Domain.Commands
{
    public class UpdateQuoteStatusCommand : QuoteCommand
    {
        public override bool IsValid(){
            return true;
        }
    }
}