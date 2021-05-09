using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class UpdateUserEmailVerifyCommandValidation : UserValidation<UpdateUserEmailVerifyCommand> {
        public UpdateUserEmailVerifyCommandValidation () {
            ValidateId ();
        }
    }
}