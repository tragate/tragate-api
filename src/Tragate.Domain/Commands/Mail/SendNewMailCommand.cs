using Tragate.Domain.Validations.Mail;

namespace Tragate.Domain.Commands
{
    public class SendNewMailCommand : MailCommand
    {
        public override bool IsValid(){
            ValidationResult = new SendNewMailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}