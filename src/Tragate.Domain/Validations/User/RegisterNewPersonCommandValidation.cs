using Tragate.Domain.Commands;

namespace Tragate.Domain.Validations {
    public class RegisterNewPersonCommandValidation : UserValidation<RegisterNewPersonCommand> {
        public RegisterNewPersonCommandValidation () {
            ValidatePasswordMatch ();
            ValidateEmail ();
            ValidateFullName ();
            ValidateLocationId ();
            ValidateCountryId ();
            ValidateStateId ();
            ValidatePhone();
        }
    }
}