using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations
{
    public class RegisterNewExternalUserCommandValidation : UserValidation<RegisterNewExternalUserCommand>
    {
        public RegisterNewExternalUserCommandValidation(){
            ValidateFullName();
            ValidateEmail();
            ValidateStateId();
            ValidateCountryId();
            ValidateRegisterType();
            ValidateExternalUserId();
            ValidatePhone();
        }
    }
}