using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class ForgotPasswordCommandValidation : UserValidation<ForgotPasswordCommand> {
        public ForgotPasswordCommandValidation () {
            ValidateEmail ();
        }
    }
}