using Tragate.Domain.Commands;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.Validations
{
    public class ChangeEmailCommandValidation : UserValidation<ChangeEmailCommand>
    {
        public ChangeEmailCommandValidation(){
            ValidateId();
            ValidateEmail();
            ValidatePassword();
        }
    }
}