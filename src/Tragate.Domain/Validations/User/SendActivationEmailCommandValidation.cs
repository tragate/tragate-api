using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class SendActivationEmailCommandValidation : UserValidation<SendActivationEmailCommand>
    {
        public SendActivationEmailCommandValidation(){
            ValidateEmail();
        }
    }
}