using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class ResetPasswordCommanValidation : UserValidation<ResetPasswordCommand>
    {
        public ResetPasswordCommanValidation(){
            ValidatePasswordMatch();
        }
    }
}