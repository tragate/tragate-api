using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class CompleteSignUpCommandValidation : UserValidation<CompleteSignUpCommand>
    {
        public CompleteSignUpCommandValidation(){
            ValidateFullName();
            ValidatePassword();
            ValidatePasswordMatch();
        }
    }
}