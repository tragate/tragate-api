using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations.Mail
{
    public class SendNewMailCommandValidation : MailValidation<MailCommand>
    {
        public SendNewMailCommandValidation(){
            ValidateMailName();
            ValidateMailTitle();
            ValidateMailList();
        }
    }
}